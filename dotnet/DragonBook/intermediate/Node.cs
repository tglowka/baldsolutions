using DragonBook.lexer;
using DragonBook.parser;
using Type = DragonBook.symbols.Type;
using Array = DragonBook.symbols.Array;

namespace DragonBook.intermediate;

public abstract class Node
{
    private readonly int _lexerLine = Lexer.Line;
    private static int _labelNo;

    protected static void Emit(string message)
        => OutputBuilder.Emit(message);

    public void Error(string message)
        => throw new Exception($"Near line {_lexerLine}: {message} ");

    public static void EmitLabel(int labelNo)
        => OutputBuilder.EmitLabel(labelNo);

    public static int NewLabel()
        => ++_labelNo;
}

public abstract class Expr : Node
{
    protected readonly Token? Op;
    public Type? Type;

    protected Expr(Token? token, Type? type)
    {
        Op = token;
        Type = type;
    }

    protected static void EmitJumps(string test, int trueLabelNo, int falseLabelNo)
    {
        if (trueLabelNo != 0 && falseLabelNo != 0)
        {
            Emit($"if {test} goto L{trueLabelNo}");
            Emit($"goto L{falseLabelNo}");
        }
        else if (trueLabelNo != 0)
        {
            Emit($"if {test} goto L{trueLabelNo}");
        }
        else if (falseLabelNo != 0)
        {
            Emit($"if false {test} goto L{falseLabelNo}");
        }
    }

    public virtual Expr Gen() => this;

    public virtual Expr Reduce() => this;

    public virtual void Jumping(int trueLabelNo, int falseLabelNo) => EmitJumps(ToString(), trueLabelNo, falseLabelNo);

    public override string ToString() => Op.ToString();
}

public class Id : Expr
{
    // public int Offset;

    public Id(Token? id, Type? type, int relativeAddress) : base(id, type)
    {
    } // => Offset = relativeAddress;
}

public abstract class Op : Expr
{
    protected Op(Token? token, Type? type) : base(token, type)
    {
    }

    public override Expr Reduce()
    {
        var expr = Gen();
        var temp = new Temp(Type);
        Emit($"{temp} = {expr}");
        return temp;
    }
}

public class Arith : Op
{
    private readonly Expr _leftExpr;
    private readonly Expr _rightExpr;

    public Arith(Token? token, Expr leftExpr, Expr rightExpr) : base(token, null)
    {
        _leftExpr = leftExpr;
        _rightExpr = rightExpr;
        Type = Type.Max(_leftExpr.Type, _rightExpr.Type);
        if (Type == null)
        {
            Error("type error");
        }
    }

    public override Expr Gen() => new Arith(Op, _leftExpr.Reduce(), _rightExpr.Reduce());

    public override string ToString() => $"{_leftExpr} {Op} {_rightExpr}";
}

public class Temp : Expr
{
    private static int _count;
    private readonly int _number;

    public Temp(Type? type) : base(Word.Temp, type) => _number = ++_count;

    public override string ToString() => $"t{_number}";
}

public class Unary : Op
{
    private readonly Expr _expr;

    public Unary(Token? token, Expr expr) : base(token, null)
    {
        _expr = expr;
        Type = Type.Max(Type.Int, _expr.Type);
        if (Type == null)
        {
            Error("type error");
        }
    }

    public override Expr Gen() => new Unary(Op, _expr.Reduce());

    public override string ToString() => $"{Op} {_expr}";
}

public class Constant : Expr
{
    public static readonly Constant True = new(Word.True, Type.Bool);
    public static readonly Constant False = new(Word.False, Type.Bool);

    public Constant(Token? token, Type? type) : base(token, type)
    {
    }

    public Constant(int value) : base(new Num(value), Type.Int)
    {
    }

    public override void Jumping(int trueLabelNo, int falseLabelNo)
    {
        if (this == True && trueLabelNo != 0)
        {
            Emit($"goto L{trueLabelNo}");
        }
        else if (this == False && falseLabelNo != 0)
        {
            Emit($"goto L{falseLabelNo}");
        }
    }
}

public abstract class Logical : Expr
{
    protected readonly Expr LeftExpr;
    protected readonly Expr RightExpr;

    protected Logical(Token? token, Expr leftExpr, Expr rightExpr) : base(token, null)
    {
        LeftExpr = leftExpr;
        RightExpr = rightExpr;
        Type = Check(LeftExpr.Type, RightExpr.Type);
        if (Type == null)
        {
            Error("type error");
        }
    }

    protected virtual Type? Check(Type? type1, Type? type2)
    {
        if (type1 == Type.Bool && type2 == Type.Bool)
        {
            return Type.Bool;
        }

        return null;
    }

    public override Expr Gen()
    {
        var f = NewLabel();
        var a = NewLabel();
        var temp = new Temp(Type);
        Jumping(0, f);
        Emit($"{temp} = true");
        Emit($"goto L{a}");
        EmitLabel(f);
        Emit($"{temp} = false");
        EmitLabel(a);
        return temp;
    }

    public override string ToString() => $"{LeftExpr} {Op} {RightExpr}";
}

public class Or : Logical
{
    public Or(Token? token, Expr leftExpr, Expr rightExpr) : base(token, leftExpr, rightExpr)
    {
    }

    public override void Jumping(int t, int f)
    {
        var label = t != 0 ? t : NewLabel();
        LeftExpr.Jumping(label, 0);
        RightExpr.Jumping(t, f);
        if (t == 0)
        {
            EmitLabel(label);
        }
    }
}

public class And : Logical
{
    public And(Token? tok, Expr leftExpr, Expr rightExpr) : base(tok, leftExpr, rightExpr)
    {
    }

    public override void Jumping(int t, int f)
    {
        var label = f != 0 ? f : NewLabel();
        LeftExpr.Jumping(0, label);
        RightExpr.Jumping(t, f);
        if (f == 0)
        {
            EmitLabel(label);
        }
    }
}

public class Not : Logical
{
    public Not(Token? token, Expr rightExpr) : base(token, rightExpr, rightExpr)
    {
    }

    public override void Jumping(int t, int f) => RightExpr.Jumping(f, t);
    public override string ToString() => $"{Op} {RightExpr}";
}

public class Rel : Logical
{
    public Rel(Token? token, Expr leftExpr, Expr rightExpr) : base(token, leftExpr, rightExpr)
    {
    }

    protected override Type? Check(Type? p1, Type? p2)
    {
        if (p1 is Array || p2 is Array)
        {
            return null;
        }

        if (p1 == p2)
        {
            return Type.Bool;
        }

        return null;
    }

    public override void Jumping(int t, int f)
    {
        var left = LeftExpr.Reduce();
        var right = RightExpr.Reduce();
        var test = $"{left} {Op} {right}";
        EmitJumps(test, t, f);
    }
}

public class Access : Op
{
    public readonly Id Array;
    public readonly Expr Index;

    public Access(Id id, Expr expr, Type? type) : base(new Word("[]", Tag.Index), type)
    {
        Array = id;
        Index = expr;
    }

    public override Expr Gen() 
        => new Access(Array, Index.Reduce(), Type);

    public override void Jumping(int t, int f) 
        => EmitJumps(Reduce().ToString(), t, f);

    public override string ToString() 
        => $"{Array} [{Index}]";
}

public class Stmt : Node
{
    public static readonly Stmt Null = new();
    public static Stmt Enclosing = Null;

    public int After;

    public virtual void Gen(int beginLabel, int afterLabel)
    {
    }
}

public class If : Stmt
{
    private readonly Expr _expr;
    private readonly Stmt _stmt;

    public If(Expr x, Stmt s)
    {
        _expr = x;
        _stmt = s;
        if (_expr.Type != Type.Bool)
        {
            _expr.Error("Boolean required in if");
        }
    }

    public override void Gen(int beginLabel, int afterLabel)
    {
        var label = NewLabel();
        _expr.Jumping(0, afterLabel);
        EmitLabel(label);
        _stmt.Gen(label, afterLabel);
    }
}

public class Else : Stmt
{
    private readonly Expr _expr;
    private readonly Stmt _stmt1;
    private readonly Stmt _stmt2;

    public Else(Expr x, Stmt s1, Stmt s2)
    {
        _expr = x;
        _stmt1 = s1;
        _stmt2 = s2;
        if (_expr.Type != Type.Bool) _expr.Error("boolean required in if");
    }

    public override void Gen(int beginLabel, int afterLabel)
    {
        var label1 = NewLabel();
        var label2 = NewLabel();
        _expr.Jumping(0, label2);
        EmitLabel(label1);
        _stmt1.Gen(label1, afterLabel);
        EmitLabel(label2);
        _stmt2.Gen(label2, afterLabel);
    }
}

public class While : Stmt
{
    private Expr? _expr;
    private Stmt? _stmt;

    public void Init(Expr expr, Stmt stmt)
    {
        _expr = expr;
        _stmt = stmt;
        if (_expr.Type != Type.Bool)
        {
            _expr.Error("boolean required in if");
        }
    }

    public override void Gen(int beginLabel, int afterLabel)
    {
        After = afterLabel;
        _expr?.Jumping(0, afterLabel);
        var label = NewLabel();
        EmitLabel(label);
        _stmt?.Gen(label, beginLabel);
        Emit($"goto L{beginLabel}");
    }
}

public class Do : Stmt
{
    private Expr? _expr;
    private Stmt? _stmt;

    public void Init(Stmt stmt, Expr expr)
    {
        _expr = expr;
        _stmt = stmt;
        if (_expr.Type != Type.Bool)
        {
            _expr.Error("boolean required in if");
        }
    }

    public override void Gen(int beginLabel, int afterLabel)
    {
        After = afterLabel;
        var label = NewLabel();
        _stmt?.Gen(beginLabel, label);
        EmitLabel(label);
        _expr?.Jumping(beginLabel, 0);
    }
}

public class Set : Stmt
{
    private readonly Id _id;
    private readonly Expr _expr;

    public Set(Id id, Expr expr)
    {
        _id = id;
        _expr = expr;
        if (Check(_id.Type, _expr.Type) == null)
        {
            Error("type error");
        }
    }

    private static Type? Check(Type? type1, Type? type2)
    {
        if (Type.IsNumeric(type1) && Type.IsNumeric(type2))
        {
            return type2;
        }

        if (type1 == Type.Bool && type2 == Type.Bool)
        {
            return type2;
        }

        return null;
    }

    public override void Gen(int beginLabel, int afterLabel) => Emit($"{_id} = {_expr.Gen()}");
}

public class SetArrayElem : Stmt
{
    private readonly Id _array;
    private readonly Expr _index;
    private readonly Expr _expr;

    public SetArrayElem(Access access, Expr expr)
    {
        _array = access.Array;
        _index = access.Index;
        _expr = expr;
        if (Check(access.Type, _expr.Type) == null)
        {
            Error("type error");
        }
    }

    private static Type? Check(Type? type1, Type? type2)
    {
        if (type1 is Array || type2 is Array)
        {
            return null;
        }

        if (type1 == type2)
        {
            return type2;
        }

        if (Type.IsNumeric(type1) && Type.IsNumeric(type2))
        {
            return type2;
        }

        return null;
    }

    public override void Gen(int beginLabel, int afterLabel)
    {
        var index = _index.Reduce().ToString();
        var expr = _expr.Reduce().ToString();
        Emit($"{_array} [{index}] = {expr}");
    }
}

public class StmtSeq : Stmt
{
    private readonly Stmt _stmt1;
    private readonly Stmt _stmt2;

    public StmtSeq(Stmt s1, Stmt s2)
    {
        _stmt1 = s1;
        _stmt2 = s2;
    }

    public override void Gen(int beginLabel, int afterLabel)
    {
        if (_stmt1 == Null)
        {
            _stmt2.Gen(beginLabel, afterLabel);
        }
        else if (_stmt2 == Null)
        {
            _stmt1.Gen(beginLabel, afterLabel);
        }
        else
        {
            var label = NewLabel();
            _stmt1.Gen(beginLabel, label);
            EmitLabel(label);
            _stmt2.Gen(label, afterLabel);
        }
    }
}

public class Break : Stmt
{
    private readonly Stmt _stmt;

    public Break()
    {
        if (Enclosing == Null)
        {
            Error("unenclosed break");
        }

        _stmt = Enclosing;
    }

    public override void Gen(int beginLabel, int afterLabel) => Emit($"goto L{_stmt.After}");
}
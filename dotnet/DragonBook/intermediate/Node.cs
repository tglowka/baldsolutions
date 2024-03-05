using DragonBook.lexer;
using Type = DragonBook.symbols.Type;

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

public abstract class TypedExpr: Node
{
    protected readonly Token Op;
    public readonly Type Type;
    
    protected TypedExpr(Token token, Type type)
    {
        Op = token;
        Type = type;
    }

    protected static void EmitJumps(string predicate, int trueLabelNo, int falseLabelNo)
    {
        if (trueLabelNo != 0 && falseLabelNo != 0)
        {
            Emit($"if {predicate} goto L{trueLabelNo}");
            Emit($"goto L{falseLabelNo}");
        }
        else if (trueLabelNo != 0)
        {
            Emit($"if {predicate} goto L{trueLabelNo}");
        }
        else if (falseLabelNo != 0)
        {
            Emit($"if false {predicate} goto L{falseLabelNo}");
        }
    }

    public virtual TypedExpr Gen()
        => this;

    public virtual TypedExpr Reduce()
        => this;
    
    public virtual void Jumping(int trueLabelNo, int falseLabelNo)
        => EmitJumps(ToString(), trueLabelNo, falseLabelNo);

    public override string ToString()
        => Op.ToString();
}

public class Operation : TypedExpr
{
    protected Operation(Token token, Type type) : base(token, type)
    {
    }
    
    public override TypedExpr Reduce()
    {
        var expr = Gen();
        var temp = new Temp(Type);
        Emit($"{temp} = {expr}");
        return temp;
    }
}

public class Id : TypedExpr
{
    public Id(Token id, Type type) : base(id, type)
    {
    }
}

public class Arith : Operation
{
    private readonly TypedExpr _leftExpr;
    private readonly TypedExpr _rightExpr;

    public Arith(Token token, TypedExpr leftExpr, TypedExpr rightExpr) : base(token, leftExpr.Type)
    {
        AssertType(leftExpr, rightExpr);
        _leftExpr = leftExpr;
        _rightExpr = rightExpr;
    }

    private void AssertType(TypedExpr leftExpr, TypedExpr rightExpr)
    {
        if (leftExpr.Type != rightExpr.Type)
        {
            Error("type error");
        }
    }

    public override TypedExpr Gen()
        => new Arith(Op, _leftExpr.Reduce(), _rightExpr.Reduce());

    public override string ToString()
        => $"{_leftExpr} {Op} {_rightExpr}";
}

public class Temp : TypedExpr
{
    private static int _count;
    private readonly int _number;

    public Temp(Type type) : base(Word.Temp, type)
        => _number = ++_count;

    public override string ToString()
        => $"t{_number}";
}

public class Unary : Operation
{
    private readonly TypedExpr _expr;

    public Unary(Token token, TypedExpr expr) : base(token, expr.Type)
    {
        AssertType(expr);
        _expr = expr;
    }

    private void AssertType(TypedExpr expr)
    {
        if (expr.Type != Type.Int && expr.Type != Type.Float)
        {
            Error("type error");
        }
    }

    public override TypedExpr Gen()
        => new Unary(Op, _expr.Reduce());

    public override string ToString()
        => $"{Op} {_expr}";
}

public class Constant : TypedExpr
{
    public static readonly Constant True = new(Word.True, Type.Bool);
    public static readonly Constant False = new(Word.False, Type.Bool);

    public Constant(Token token, Type type) : base(token, type)
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

public abstract class Logical : TypedExpr
{
    protected readonly TypedExpr LeftExpr;
    protected readonly TypedExpr RightExpr;

    protected Logical(Token token, TypedExpr leftExpr, TypedExpr rightExpr) : base(token, Type.Bool)
    {
        AssertType(leftExpr, rightExpr);
        LeftExpr = leftExpr;
        RightExpr = rightExpr;
    }

    private void AssertType(TypedExpr left, TypedExpr right)
    {
        if (left.Type != right.Type)
        {
            Error("type error");
        }
    }

    public override TypedExpr Gen()
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

    public override string ToString()
        => $"{LeftExpr} {Op} {RightExpr}";
}

public class Or : Logical
{
    public Or(Token token, TypedExpr leftExpr, TypedExpr rightExpr) : base(token, leftExpr, rightExpr)
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
    public And(Token token, TypedExpr leftExpr, TypedExpr rightExpr) : base(token, leftExpr, rightExpr)
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
    public Not(Token token, TypedExpr rightExpr) : base(token, rightExpr, rightExpr)
    {
    }

    public override void Jumping(int t, int f) => RightExpr.Jumping(f, t);
    public override string ToString() => $"{Op} {RightExpr}";
}

public class Rel : Logical
{
    public Rel(Token token, TypedExpr leftExpr, TypedExpr rightExpr) : base(token, leftExpr, rightExpr)
        => AssertType(leftExpr, rightExpr);

    private void AssertType(TypedExpr leftExpr, TypedExpr rightExpr)
    {
        if (!Type.IsNumeric(leftExpr.Type) || !Type.IsNumeric(rightExpr.Type))
        {
            Error("type error");
        }
    }

    public override void Jumping(int t, int f)
    {
        var left = LeftExpr.Reduce();
        var right = RightExpr.Reduce();
        var test = $"{left} {Op} {right}";
        EmitJumps(test, t, f);
    }
}

public class ArrayAccess : Operation
{
    public readonly Id Id;
    public readonly TypedExpr Index;

    public ArrayAccess(Id id, TypedExpr expr, Type type) : base(new Word("[]", Tag.Index), type)
    {
        Id = id;
        Index = expr;
    }

    public override TypedExpr Gen()
        => new ArrayAccess(Id, Index.Reduce(), Type);

    public override void Jumping(int t, int f)
        => EmitJumps(Reduce().ToString(), t, f);

    public override string ToString()
        => $"{Id} [{Index}]";
}

public abstract class Stmt : Node
{
    public static Stmt Enclosing = new EmptyStmt();

    public int After;

    public virtual void Gen(int beginLabel, int afterLabel)
    {
    }
}

public class EmptyStmt : Stmt
{
}

public class If : Stmt
{
    private readonly TypedExpr _expr;
    private readonly Stmt _stmt;

    public If(TypedExpr expr, Stmt stmt)
    {
        AssertType(expr);
        _expr = expr;
        _stmt = stmt;
    }

    private void AssertType(TypedExpr expr)
    {
        if (expr.Type != Type.Bool)
        {
            Error("Boolean required in if");
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

public class IfElse : Stmt
{
    private readonly TypedExpr _expr;
    private readonly Stmt _stmt1;
    private readonly Stmt _stmt2;

    public IfElse(TypedExpr expr, Stmt stmt1, Stmt stmt2)
    {
        AssertType(expr);
        _expr = expr;
        _stmt1 = stmt1;
        _stmt2 = stmt2;
        if (_expr.Type != Type.Bool) _expr.Error("boolean required in if");
    }

    private void AssertType(TypedExpr expr)
    {
        if (expr.Type != Type.Bool)
        {
            Error("Boolean required in if");
        }
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
    private TypedExpr _expr;
    private Stmt? _stmt;

    public void Init(TypedExpr expr, Stmt stmt)
    {
        AssertType(expr);
        _expr = expr;
        _stmt = stmt;
    }

    private void AssertType(TypedExpr expr)
    {
        if (expr.Type != Type.Bool)
        {
            Error("Boolean required in if");
        }
    }
    
    public override void Gen(int beginLabel, int afterLabel)
    {
        After = afterLabel;
        _expr.Jumping(0, afterLabel);
        var label = NewLabel();
        EmitLabel(label);
        _stmt?.Gen(label, beginLabel);
        Emit($"goto L{beginLabel}");
    }
}

public class Do : Stmt
{
    private TypedExpr _expr;
    private Stmt? _stmt;

    public void Init(Stmt stmt, TypedExpr expr)
    {
        AssertType(expr);
        _expr = expr;
        _stmt = stmt;
    }
    
    private void AssertType(TypedExpr expr)
    {
        if (expr.Type != Type.Bool)
        {
            Error("Boolean required in if");
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
    private readonly TypedExpr _expr;

    public Set(Id id, TypedExpr expr)
    {
        AssertType(id, expr);
        _id = id;
        _expr = expr;
    }

    private void AssertType(TypedExpr id, TypedExpr expr)
    {
        if (id.Type != expr.Type)
        {
            Error("type error");
        }
    }

    public override void Gen(int beginLabel, int afterLabel)
        => Emit($"{_id} = {_expr.Gen()}");
}

public class SetArrayElem : Stmt
{
    private readonly Id _id;
    private readonly TypedExpr _index;
    private readonly TypedExpr _expr;

    public SetArrayElem(ArrayAccess arrayAccess, TypedExpr expr)
    {
        AssertType(arrayAccess, expr);
        _id = arrayAccess.Id;
        _index = arrayAccess.Index;
        _expr = expr;
    }
    
    private void AssertType(ArrayAccess arrayAccess, TypedExpr expr)
    {
        if (!Type.AreEqual(arrayAccess.Id.Type, expr.Type))
        {
            Error("type error");
        }
    }

    public override void Gen(int beginLabel, int afterLabel)
    {
        var index = _index.Reduce().ToString();
        var expr = _expr.Reduce().ToString();
        Emit($"{_id} [{index}] = {expr}");
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
        if (_stmt1 is EmptyStmt)
        {
            _stmt2.Gen(beginLabel, afterLabel);
        }
        else if (_stmt2 is EmptyStmt)
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
        if (Enclosing is EmptyStmt)
        {
            Error("unenclosed break");
        }

        _stmt = Enclosing;
    }

    public override void Gen(int beginLabel, int afterLabel) 
        => Emit($"goto L{_stmt.After}");
}
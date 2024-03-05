using DragonBook.intermediate;
using DragonBook.lexer;
using DragonBook.symbols;
using Array = DragonBook.symbols.Array;
using Type = DragonBook.symbols.Type;

namespace DragonBook.parser;

public class Parser
{
    private readonly Lexer _lexer;
    private Token _look;
    private Env? _top;
    private int _used;

    public Parser(Lexer lexer)
    {
        _lexer = lexer;
        Move();
    }

    private void Move()
        => _look = _lexer.Scan();

    private static void Error(string s)
        => throw new Exception($"Near line {Lexer.Line}:{s}");

    private void Match(int tag)
    {
        if (_look.Tag == tag)
        {
            Move();
        }
        else
        {
            Error("syntax error");
        }
    }

    public string Program()
    {
        var stmt = Block();
        var begin = Node.NewLabel();
        var after = Node.NewLabel();
        Node.EmitLabel(begin);
        stmt.Gen(begin, after);
        Node.EmitLabel(after);
        return OutputBuilder.Build();
    }

    private Stmt Block()
    {
        Match('{');
        var savedEnv = _top;
        _top = new Env(_top);
        Decls();
        var stmts = StmtSeq();
        Match('}');
        _top = savedEnv;
        return stmts;
    }

    private void Decls()
    {
        while (_look.Tag == Tag.Basic)
        {
            var type = Type();
            var token = _look;
            Match(Tag.Id);
            Match(';');
            var id = new Id(token, type);
            _top?.Put(token, id);
            _used += type.Width;
        }
    }

    private Type Type()
    {
        var type = (Type)_look;
        Match(Tag.Basic);

        return _look.Tag != '['
            ? type
            : Dims(type);
    }

    private Type Dims(Type? type)
    {
        Match('[');
        var token = _look;
        Match(Tag.Num);
        Match(']');
        if (_look.Tag == '[')
        {
            type = Dims(type);
        }

        return new Array(((Num)token).Value, type!);
    }

    private Stmt StmtSeq() =>
        _look.Tag == '}'
            ? new EmptyStmt()
            : new StmtSeq(Stmt(), StmtSeq());

    private Stmt Stmt()
    {
        TypedExpr condition;
        Stmt stmt, savedStmt;

        switch (_look.Tag)
        {
            case ';':
                Move();
                return new EmptyStmt();
            case Tag.If:
                Match(Tag.If);
                Match('(');
                condition = Boolean();
                Match(')');
                stmt = Stmt();
                if (_look.Tag != Tag.Else)
                {
                    return new If(condition, stmt);
                }

                Match(Tag.Else);
                var elseStmt = Stmt();
                return new IfElse(condition, stmt, elseStmt);
            case Tag.While:
                var whileNode = new While();
                savedStmt = intermediate.Stmt.Enclosing;
                intermediate.Stmt.Enclosing = whileNode;
                Match(Tag.While);
                Match('(');
                condition = Boolean();
                Match(')');
                stmt = Stmt();
                whileNode.Init(condition, stmt);
                intermediate.Stmt.Enclosing = savedStmt;
                return whileNode;
            case Tag.Do:
                var doNode = new Do();
                savedStmt = intermediate.Stmt.Enclosing;
                intermediate.Stmt.Enclosing = doNode;
                Match(Tag.Do);
                stmt = Stmt();
                Match(Tag.While);
                Match('(');
                condition = Boolean();
                Match(')');
                Match(';');
                doNode.Init(stmt, condition);
                intermediate.Stmt.Enclosing = savedStmt;
                return doNode;
            case Tag.Break:
                Match(Tag.Break);
                Match(';');
                return new Break();
            case '{':
                return Block();
            default:
                return Assign();
        }
    }

    private Stmt Assign()
    {
        Stmt stmt;
        var token = _look;
        Match(Tag.Id);
        var id = _top!.Get(token);
        if (id == null)
        {
            Error($"{token} undeclared");
        }

        if (_look.Tag == '=')
        {
            Move();
            stmt = new Set(id!, Boolean());
        }
        else
        {
            var access = Offset(id!);
            Match('=');
            stmt = new SetArrayElem(access, Boolean());
        }

        Match(';');
        return stmt;
    }

    private TypedExpr Boolean()
    {
        var expr = Join();
        while (_look.Tag == Tag.Or)
        {
            var token = _look;
            Move();
            expr = new Or(token, expr, Join());
        }

        return expr;
    }

    private TypedExpr Join()
    {
        var expr = Equality();
        while (_look.Tag == Tag.And)
        {
            var token = _look;
            Move();
            expr = new And(token, expr, Equality());
        }

        return expr;
    }

    private TypedExpr Equality()
    {
        var expr = Rel();
        while (_look.Tag == Tag.Eq || _look.Tag == Tag.Ne)
        {
            var token = _look;
            Move();
            expr = new Rel(token, expr, Rel());
        }

        return expr;
    }

    private TypedExpr Rel()
    {
        var expr = Expr();
        switch (_look.Tag)
        {
            case '<':
            case Tag.Le:
            case Tag.Ge:
            case '>':
                var token = _look;
                Move();
                return new Rel(token, expr, Expr());
            default:
                return expr;
        }
    }

    private TypedExpr Expr()
    {
        var expr = Term();
        while (_look.Tag is '+' or '-')
        {
            var token = _look;
            Move();
            expr = new Arith(token, expr, Term());
        }

        return expr;
    }

    private TypedExpr Term()
    {
        var expr = Unary();
        while (_look.Tag is '*' or '/')
        {
            var token = _look;
            Move();
            expr = new Arith(token, expr, Unary());
        }

        return expr;
    }

    private TypedExpr Unary()
    {
        switch (_look.Tag)
        {
            case '-':
                Move();
                return new Unary(Word.Minus, Unary());
            case '!':
            {
                var token = _look;
                Move();
                return new Not(token, Unary());
            }
            default:
                return Factor();
        }
    }

    private TypedExpr Factor()
    {
        TypedExpr expr;
        switch (_look.Tag)
        {
            case '(':
                Move();
                expr = Boolean();
                Match(')');
                return expr;
            case Tag.Num:
                expr = new Constant(_look, symbols.Type.Int);
                Move();
                return expr;
            case Tag.Real:
                expr = new Constant(_look, symbols.Type.Float);
                Move();
                return expr;
            case Tag.True:
                expr = Constant.True;
                Move();
                return expr;
            case Tag.False:
                expr = Constant.False;
                Move();
                return expr;
            case Tag.Id:
                var id = _top!.Get(_look);
                if (id == null)
                {
                    Error($"{_look} undeclared");
                }
                Move();
                if (_look.Tag != '[')
                {
                    return id!;
                }
                
                return Offset(id!);
            default:
                Error("syntax error");
                return null!;
        }
    }

    private ArrayAccess Offset(Id id)
    {
        TypedExpr expr, widthExpr, t1, t2, loc;
        var type = id.Type;
        Match('[');
        expr = Boolean();
        Match(']');
        type = ((Array)type!).Type;
        widthExpr = new Constant(type!.Width);
        t1 = new Arith(new Token('*'), expr, widthExpr);
        loc = t1;
        while (_look.Tag == '[')
        {
            Match('[');
            expr = Boolean();
            Match(']');
            type = ((Array)type).Type;
            widthExpr = new Constant(type!.Width);
            t1 = new Arith(new Token('*'), expr, widthExpr);
            t2 = new Arith(new Token('+'), loc, t1);
            loc = t2;
        }

        return new ArrayAccess(id, loc, type);
    }
}
using DragonBook.intermediate;
using DragonBook.lexer;
using Tags = DragonBook.lexer.Tag;

namespace DragonBook.symbols;

public class Env
{
    private readonly Dictionary<Token, Id> _table;
    private readonly Env? _prev;

    public Env(Env? prevEnv)
    {
        _table = new Dictionary<Token, Id>();
        _prev = prevEnv;
    }

    public void Put(Token token, Id id) => _table.Add(token, id);

    public Id? Get(Token token)
    {
        for (var env = this; env != null; env = env._prev)
        {
            if (env._table.TryGetValue(token, out var found))
            {
                return found;
            }
        }

        return null;
    }
}

public record Type(string Lexeme, int Tag, int Width) : Word(Lexeme, Tag)
{
    public static readonly Type Int = new("int", Tags.Basic, 4);
    public static readonly Type Float = new("float", Tags.Basic, 8);
    public static readonly Type Char = new("char", Tags.Basic, 1);
    public static readonly Type Bool = new("bool", Tags.Basic, 1);

    public static bool IsNumeric(Type? type)
        => type == Char || type == Int || type == Float;

    public static Type? Max(Type? left, Type? right)
    {
        if (!IsNumeric(left) || !IsNumeric(right))
        {
            return null;
        }

        if (left == Float || right == Float)
        {
            return Float;
        }

        if (left == Int || right == Int)
        {
            return Int;
        }

        return Char;
    }
}

public record Array : Type
{
    private readonly int _size = 1;
    public readonly Type? Of;

    public Array(int size, Type type) : base("[]", Tags.Index, size * type.Width)
    {
        _size = size;
        Of = type;
    }

    public override string ToString() => $"[{_size}] {Of}";
}
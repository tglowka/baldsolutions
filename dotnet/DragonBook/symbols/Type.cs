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

    public static bool AreEqual(Type type1, Type type2)
        => type1.Lexeme == type2.Lexeme;
}

public sealed record Array : Type
{
    private readonly int _size = 1;
    public readonly Type Type;

    public Array(int size, Type type) : base(type.Lexeme, Tags.Index, size * type.Width)
    {
        _size = size;
        Type = type;
    }

    public override string ToString() => $"[{_size}] {Type}";
}
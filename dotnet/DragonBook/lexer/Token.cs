
namespace DragonBook.lexer;

public record Token(int Tag)
{
    public override string ToString() => ((char)Tag).ToString();
}

public record Num(int Value) : Token(lexer.Tag.Num)
{
    public override string ToString() => Value.ToString();
}

public record Real(float Value) : Token(lexer.Tag.Real)
{
    public override string ToString() => Value.ToString();
}

public record Word(string Lexeme, int Tag) : Token(Tag)
{
    public static readonly Word And = new("&&", lexer.Tag.And);
    public static readonly Word Or = new("||", lexer.Tag.Or);
    public static readonly Word Eq = new("==", lexer.Tag.Eq);
    public static readonly Word Ne = new("!=", lexer.Tag.Ne);
    public static readonly Word Le = new("<=", lexer.Tag.Le);
    public static readonly Word Ge = new(">=", lexer.Tag.Ge);
    public static readonly Word Minus = new("minus", lexer.Tag.Minus);
    public static readonly Word True = new("true", lexer.Tag.True);
    public static readonly Word False = new("false", lexer.Tag.False);
    public static readonly Word If = new("if", lexer.Tag.If);
    public static readonly Word Else = new("else", lexer.Tag.Else);
    public static readonly Word While = new("while", lexer.Tag.While);
    public static readonly Word Do = new("do", lexer.Tag.Do);
    public static readonly Word Break = new("break", lexer.Tag.Break);
    public static readonly Word Temp = new("t", lexer.Tag.Temp);

    public override string ToString() 
        => Lexeme;
};
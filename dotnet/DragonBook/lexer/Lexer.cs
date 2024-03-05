using System.Text;
using Type = DragonBook.symbols.Type;

namespace DragonBook.lexer;

public class Lexer
{
    public static int Line = 1;

    private int _idx;
    private char _peek = ' ';
    private readonly string _program;

    private readonly Dictionary<string, Word?> _words = new();

    public Lexer(string program)
    {
        _program = program;
        Reserve(Word.If);
        Reserve(Word.Else);
        Reserve(Word.While);
        Reserve(Word.Do);
        Reserve(Word.Break);
        Reserve(Word.True);
        Reserve(Word.False);
        Reserve(Type.Int);
        Reserve(Type.Char);
        Reserve(Type.Float);
        Reserve(Type.Bool);
    }

    private void Reserve(Word word) => _words.Add(word.Lexeme, word);

    private void ReadChar()
    {
        if (_idx < _program.Length)
        {
            _peek = _program[_idx++];
        }
        else
        {
            _peek = (char)0x00;
        }
        //(char)Console.Read();
    }

    private char ReadNextChar() =>
        _idx < _program.Length
            ? _program[_idx++]
            : (char)0x00;

    //(char)Console.Read();
    private bool ReadChar(char c)
    {
        ReadChar();
        if (_peek != c)
        {
            return false;
        }

        _peek = ' ';
        return true;
    }

    public Token Scan()
    {
        for (;; ReadChar())
        {
            if (_peek is ' ' or '\t' or '\r')
            {
                continue;
            }
            if (_peek == '\n')
            {
                ++Line;
            }
            else
            {
                break;
            }
        }

        switch (_peek)
        {
            case '&': return ReadChar('&') ? Word.And : new Token('&');
            case '|': return ReadChar('|') ? Word.Or : new Token('|');
            case '=': return ReadChar('=') ? Word.Eq : new Token('=');
            case '!': return ReadChar('=') ? Word.Ne : new Token('!');
            case '<': return ReadChar('=') ? Word.Le : new Token('<');
            case '>': return ReadChar('=') ? Word.Ge : new Token('>');
        }

        if (char.IsDigit(_peek))
        {
            var value = 0;
            do
            {
                value = 10 * value + int.Parse(_peek.ToString());
                _peek = ReadNextChar();
            } while (char.IsDigit(_peek));

            if (_peek != '.')
            {
                return new Num(value);
            }

            float x = value;
            float d = 10;

            for (;;)
            {
                ReadChar();
                if (!char.IsDigit(_peek))
                {
                    break;
                }

                x += int.Parse(_peek.ToString()) / d;
                d *= 10;
            }

            return new Real(x);
        }

        if (char.IsLetter(_peek))
        {
            StringBuilder sb = new();
            do
            {
                sb.Append(_peek);
                ReadChar();
            } while (char.IsLetterOrDigit(_peek));

            var lexeme = sb.ToString();
            if (_words.TryGetValue(lexeme, out var word))
            {
                return word!;
            }

            word = new Word(lexeme, Tag.Id);
            _words.Add(lexeme, word);
            return word;
        }

        var token = new Token(_peek);
        _peek = ' ';
        return token;
    }
}
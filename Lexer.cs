namespace Calculator;

public class Lexer
{
    private readonly string _text;
    private int _pos;

    public Lexer(string text)
    {
        _text = text;
        _pos = 0;
    }

    private char Current => _pos < _text.Length ? _text[_pos] : '\0';
    private char Next => _pos + 1 < _text.Length ? _text[_pos + 1] : '\0';
    private void Advance() => _pos++;

    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (_pos < _text.Length)
        {
            switch (Current)
            {
                case char c when char.IsWhiteSpace(c):
                    _pos++;
                    break;
                case char c when char.IsDigit(c):
                    tokens.Add(ReadNumber());
                    break;
                case char c when char.IsLetter(c):
                    tokens.Add(ReadIdentifier());
                    break;
                case char c when IsOperatorChar(c):
                    tokens.Add(ReadOperator());
                    break;
                default:
                    throw new Exception($"Unexpected character: {Current}");
            }
        }

        tokens.Add(new Token(TokenType.EndOfInput, string.Empty));
        return tokens;
    }

    private Token ReadNumber()
    {
        var start = _pos;

        while (char.IsDigit(Current) || Current == '.')
            _pos++;

        var number = _text.Substring(start, _pos - start);
        return new Token(TokenType.Number, number);
    }

    private Token ReadIdentifier()
    {
        var start = _pos;

        while (char.IsLetterOrDigit(Current) || Current == '_')
            _pos++;

        var identifier = _text.Substring(start, _pos - start);

        return identifier switch
        {
            "true" => new Token(TokenType.True, identifier),
            "false" => new Token(TokenType.False, identifier),
            _ => new Token(TokenType.Identifier, identifier)
        };
    }

    private bool IsOperatorChar(char c)
    {
        return "+-*/()=><&|!".Contains(c);
    }

    private Token ReadOperator()
    {
        char op = Current;
        _pos++;

        switch (op)
        {
            case '+':
                return new Token(TokenType.Plus, "+");
            case '-':
                return new Token(TokenType.Minus, "-");
            case '*':
                return new Token(TokenType.Multiply, "*");
            case '/':
                return new Token(TokenType.Divide, "/");
            case '(':
                return new Token(TokenType.LeftParen, "(");
            case ')':
                return new Token(TokenType.RightParen, ")");
            case '=':
                if (Current == '=')
                {
                    Advance();
                    return new Token(TokenType.EqualEqual, "==");
                }
                return new Token(TokenType.Equal, "=");
            case '>':
                if (Current == '=')
                {
                    Advance();
                    return new Token(TokenType.GreaterEqual, ">=");
                }
                return new Token(TokenType.Greater, ">");
            case '<':
                if (Current == '=')
                {
                    Advance();
                    return new Token(TokenType.LessEqual, "<=");
                }
                return new Token(TokenType.Less, "<");
            case '!':
                if (Current == '=')
                {
                    Advance();
                    return new Token(TokenType.NotEqual, "!=");
                }
                return new Token(TokenType.Not, "!");
            case '&':
                if (Current == '&')
                {
                    Advance();
                    return new Token(TokenType.And, "&&");
                }
                break;
            case '|':
                if (Current == '|')
                {
                    Advance();
                    return new Token(TokenType.Or, "||");
                }
                break;
            default:
                throw new Exception($"Unexpected operator: {op}");
        }
        throw new Exception($"Unexpected operator: {op}");
    }
}
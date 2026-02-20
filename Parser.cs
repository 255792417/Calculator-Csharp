namespace Calculator;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _pos;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _pos = 0;
    }

    private Token Current => _pos < _tokens.Count ? _tokens[_pos] : new Token(TokenType.EndOfInput, string.Empty);
    private Token Next => _pos + 1 < _tokens.Count ? _tokens[_pos + 1] : new Token(TokenType.EndOfInput, string.Empty);

    private Token Eat(TokenType type)
    {
        if (Current.Type == type)
        {
            var token = Current;
            _pos++;
            return token;
        }
        throw new Exception($"Expected token of type {type} but found {Current.Type}");

    }

    private int GetPrecedence(TokenType tokenType)
    {
        return tokenType switch
        {
            TokenType.Equal => 1,
            TokenType.Or => 2,
            TokenType.And => 3,
            TokenType.Not => 4,
            TokenType.EqualEqual or TokenType.NotEqual or
            TokenType.Greater or TokenType.Less or
            TokenType.GreaterEqual or TokenType.LessEqual => 5,
            TokenType.Plus or TokenType.Minus => 10,
            TokenType.Multiply or TokenType.Divide => 20,
            _ => 0
        };
    }

    private ASTNode ParseExpression(int precedence = 0)
    {
        ASTNode left;

        switch (Current.Type)
        {
            case TokenType.Number:
                var numberToken = Eat(TokenType.Number);
                left = new NumberNode(double.Parse(numberToken.Value));
                break;
            case TokenType.Identifier:
                var identifierToken = Eat(TokenType.Identifier);
                left = new VariableNode(identifierToken.Value);
                break;
            case TokenType.True:
                Eat(TokenType.True);
                left = new BooleanNode(true);
                break;
            case TokenType.False:
                Eat(TokenType.False);
                left = new BooleanNode(false);
                break;
            case TokenType.Not:
                var notToken = Eat(TokenType.Not);
                var operand = ParseExpression(GetPrecedence(TokenType.Not));
                left = new UnaryOperationNode(notToken.Type, operand);
                break;
            case TokenType.LeftParen:
                Eat(TokenType.LeftParen);
                left = ParseExpression();
                Eat(TokenType.RightParen);
                break;
            default:
                throw new Exception($"Unexpected token: {Current.Type}");
        }

        while (true)
        {
            var currentPrecedence = GetPrecedence(Current.Type);
            if (currentPrecedence == 0 || currentPrecedence <= precedence)
                break;

            var operatorToken = Eat(Current.Type);

            if (operatorToken.IsRightAssociative())
                currentPrecedence--;

            var right = ParseExpression(currentPrecedence);

            if (operatorToken.Type == TokenType.Equal)
            {
                if (left is VariableNode variableNode)
                {
                    left = new AssignmentNode(variableNode.Name, right);
                }
                else
                {
                    throw new Exception("Left-hand side of assignment must be a variable");
                }
            }
            else
            {
                left = new BinaryOperationNode(left, operatorToken.Type, right);
            }
        }

        return left;
    }

    public ASTNode Parse()
    {
        return ParseExpression();
    }
}
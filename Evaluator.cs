namespace Calculator;

public class Evaluator
{
    private readonly Environment _env = new Environment();

    public object Evaluate(ASTNode node)
    {
        return Visit(node);
    }

    private object Visit(ASTNode node)
    {
        return node switch
        {
            NumberNode n => n.Value,

            BooleanNode b => b.Value,

            VariableNode v => _env.Get(v.Name),

            AssignmentNode a => VisitAssignment(a),

            BinaryOperationNode b => VisitBinary(b),

            UnaryOperationNode u => VisitUnary(u),

            _ => throw new Exception($"Unknown AST node type: {node.GetType().Name}")
        };
    }

    private object VisitAssignment(AssignmentNode a)
    {
        var value = Visit(a.Value);

        _env.Define(a.VariableName, value);

        return value;
    }

    private object VisitBinary(BinaryOperationNode b)
    {
        var left = Visit(b.Left);
        var right = Visit(b.Right);

        if (left is double leftNum && right is double rightNum)
        {
            return b.Operator switch
            {
                TokenType.EqualEqual => leftNum == rightNum,
                TokenType.NotEqual => leftNum != rightNum,
                TokenType.Greater => leftNum > rightNum,
                TokenType.Less => leftNum < rightNum,
                TokenType.GreaterEqual => leftNum >= rightNum,
                TokenType.LessEqual => leftNum <= rightNum,

                TokenType.Plus => leftNum + rightNum,
                TokenType.Minus => leftNum - rightNum,
                TokenType.Multiply => leftNum * rightNum,
                TokenType.Divide => leftNum / rightNum,
                _ => throw new Exception($"Unsupported binary operator for numbers: {b.Operator}")
            };
        }


        if (left is bool leftBool && right is bool rightBool)
        {
            return b.Operator switch
            {
                TokenType.EqualEqual => leftBool == rightBool,
                TokenType.NotEqual => leftBool != rightBool,
                TokenType.And => leftBool && rightBool,
                TokenType.Or => leftBool || rightBool,
                _ => throw new Exception($"Unsupported binary operator for booleans: {b.Operator}")
            };
        }

        throw new Exception($"Type mismatch or unsupported operand types for operator {b.Operator}");
    }

    private object VisitUnary(UnaryOperationNode u)
    {
        var operand = Visit(u.Operand);

        if (u.Operator == TokenType.Not)
        {
            if (operand is bool boolOperand)
            {
                return !boolOperand;
            }
            throw new Exception($"Unsupported operand type for 'Not' operator: {operand.GetType().Name}");
        }

        throw new Exception($"Unsupported unary operator: {u.Operator}");
    }
}

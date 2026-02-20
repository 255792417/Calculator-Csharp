namespace Calculator;

public abstract class ASTNode
{
    public void Show(int indent = 0)
    {
        Console.WriteLine(new string(' ', indent) + GetType().Name);
        foreach (var prop in GetType().GetProperties())
        {
            var value = prop.GetValue(this);
            if (value is ASTNode node)
            {
                node.Show(indent + 2);
            }
            else if (value is IEnumerable<ASTNode> nodeList)
            {
                foreach (var n in nodeList)
                {
                    n.Show(indent + 2);
                }
            }
            else
            {
                Console.WriteLine(new string(' ', indent + 2) + $"{prop.Name}: {value}");
            }
        }
    }
}

public class NumberNode : ASTNode
{
    public double Value { get; }

    public NumberNode(double value)
    {
        Value = value;
    }
}

public class VariableNode : ASTNode
{
    public string Name { get; }

    public VariableNode(string name)
    {
        Name = name;
    }
}

public class BinaryOperationNode : ASTNode
{
    public ASTNode Left { get; }
    public TokenType Operator { get; }
    public ASTNode Right { get; }

    public BinaryOperationNode(ASTNode left, TokenType operatorType, ASTNode right)
    {
        Left = left;
        Operator = operatorType;
        Right = right;
    }
}

public class UnaryOperationNode : ASTNode
{
    public TokenType Operator { get; }
    public ASTNode Operand { get; }

    public UnaryOperationNode(TokenType operatorType, ASTNode operand)
    {
        Operator = operatorType;
        Operand = operand;
    }
}

public class AssignmentNode : ASTNode
{
    public string VariableName { get; }
    public ASTNode Value { get; }

    public AssignmentNode(string variableName, ASTNode value)
    {
        VariableName = variableName;
        Value = value;
    }
}

public class BooleanNode : ASTNode
{
    public bool Value { get; }

    public BooleanNode(bool value)
    {
        Value = value;
    }
}
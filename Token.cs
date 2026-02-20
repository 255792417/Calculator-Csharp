namespace Calculator;

public enum TokenType
{
    // 值
    Number, // 123, 3.14
    Identifier, // x, y, varName

    // 操作符
    Plus, // +
    Minus, // -
    Multiply, // *
    Divide, // /
    LeftParen, // (
    RightParen, // )
    Equal, // =

    // 布尔
    True, // true
    False, // false

    // 比较
    Greater, // >
    Less, // <
    GreaterEqual, // >=
    LessEqual, // <=
    NotEqual, // /!=
    EqualEqual, // ==

    // 逻辑
    And, // &&
    Or, // ||
    Not, // /!

    // 其他
    EndOfInput
}

public class Token
{
    public TokenType Type { get; }
    public string Value { get; }

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Type}: {Value}";
    }

    public bool IsLeftAssociative()
    {
        return Type != TokenType.Equal;
    }

    public bool IsRightAssociative()
    {
        return Type == TokenType.Equal;
    }
}
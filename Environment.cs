namespace Calculator;

public class Environment
{
    private readonly Dictionary<string, object> _variables = new Dictionary<string, object>();

    public void Define(string name, object value)
    {
        _variables[name] = value;
    }

    public object Get(string name)
    {
        if (_variables.TryGetValue(name, out var value))
        {
            return value;
        }
        // 如果找不到变量，抛出异常
        throw new Exception($"Undefined variable '{name}'");
    }
}

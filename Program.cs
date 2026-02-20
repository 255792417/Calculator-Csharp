using PrettyPrompt;
using Calculator;


var prompt = new Prompt();

var evaluator = new Evaluator();
var lastTokens = new List<Token>();

while (true)
{
    var response = await prompt.ReadLineAsync().ConfigureAwait(false);

    if (!response.IsSuccess)
        break;

    var input = response.Text.Trim();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    if (input.Trim().ToLower() == "tokens")
    {
        foreach (var token in lastTokens)
        {
            Console.WriteLine(token);
        }
        continue;
    }

    try
    {
        var lexer = new Lexer(input);
        var tokens = lexer.Tokenize();
        lastTokens = tokens;

        var parser = new Parser(tokens);
        var ast = parser.Parse();

        // ast.Show(); // Uncomment to see the AST structure

        var result = evaluator.Evaluate(ast);
        Console.WriteLine(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
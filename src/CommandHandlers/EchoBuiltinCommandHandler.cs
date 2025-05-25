namespace Shell.CommandHandlers;

public class EchoBuiltinCommandHandler : IBuiltinCommandHandler
{
    public string? HandleCommand(Token[] commandArguments)
    {
        return string.Join(" ",commandArguments.Select(x => x.TokenValue));
    }
}
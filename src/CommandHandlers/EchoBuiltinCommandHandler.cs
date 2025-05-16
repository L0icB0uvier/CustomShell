namespace Shell.CommandHandlers;

public class EchoBuiltinCommandHandler : IBuiltinCommandHandler
{
    public string? HandleCommand(Token[] commandArguments)
    {
        var echoMessage = string.Join(" ",commandArguments.Select(x => x.TokenValue));
        return echoMessage;
    }
}
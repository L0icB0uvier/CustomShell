namespace Shell.CommandHandlers;

public class EchoBuiltinCommandHandler : IBuiltinCommandHandler
{
    public string? HandleCommand(string[] commandArguments)
    {
        var echoMessage = string.Join(' ',commandArguments);
        return echoMessage;
    }
}
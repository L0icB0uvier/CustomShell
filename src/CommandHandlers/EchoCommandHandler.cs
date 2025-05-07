namespace Shell;

public class EchoCommandHandler : ICommandHandler
{
    public string? HandleCommand(string[] commandArguments)
    {
        var echoMessage = string.Join(' ',commandArguments);
        return echoMessage;
    }
}
namespace Shell.CommandHandlers;

public class EchoBuiltinCommandHandler : IBuiltinCommandHandler
{
    public CommandOutput[]? HandleCommand(Token[] commandArguments)
    {
        return [new CommandOutput(string.Join(" ",commandArguments.Select(x => x.TokenValue)))];
    }
}
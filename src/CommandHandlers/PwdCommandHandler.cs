namespace Shell.CommandHandlers;

public class PwdCommandHandler : IBuiltinCommandHandler
{
    public CommandOutput[]? HandleCommand(Token[] commandArguments)
    {
        return [new CommandOutput(Directory.GetCurrentDirectory())];
    }
}
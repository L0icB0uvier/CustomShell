namespace Shell.CommandHandlers;

public class ExitBuiltinCommandHandler : IBuiltinCommandHandler
{
    public CommandOutput[]? HandleCommand(Token[] commandArguments)
    {
        Environment.Exit(0);
        return null;
    }
}
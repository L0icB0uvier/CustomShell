namespace Shell.CommandHandlers;

public class ExitBuiltinCommandHandler : IBuiltinCommandHandler
{
    public string? HandleCommand(Token[] commandArguments)
    {
        Environment.Exit(0);
        return null;
    }
}
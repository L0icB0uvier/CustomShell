namespace Shell.CommandHandlers;

public class ExitBuiltinCommandHandler : IBuiltinCommandHandler
{
    public string? HandleCommand(string[] commandArguments)
    {
        Environment.Exit(0);
        return null;
    }
}
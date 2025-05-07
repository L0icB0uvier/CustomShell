namespace Shell;

public class ExitCommandHandler : ICommandHandler
{
    public string? HandleCommand(string[] commandArguments)
    {
        Environment.Exit(0);
        return null;
    }
}
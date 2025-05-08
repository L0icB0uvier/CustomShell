namespace Shell.CommandHandlers;

public class PwdCommandHandler : IBuiltinCommandHandler
{
    public string? HandleCommand(string[] commandArguments)
    {
        return Environment.CurrentDirectory;
    }
}
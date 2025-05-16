namespace Shell.CommandHandlers;

public class PwdCommandHandler : IBuiltinCommandHandler
{
    public string? HandleCommand(Token[] commandArguments)
    {
        return Directory.GetCurrentDirectory();
    }
}
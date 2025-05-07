namespace Shell.CommandHandlers;

public interface IBuiltinCommandHandler
{
    public string? HandleCommand(string[] commandArguments);
}
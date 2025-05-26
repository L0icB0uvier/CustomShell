namespace Shell.CommandHandlers;

public interface IBuiltinCommandHandler
{
    public CommandOutput[]? HandleCommand(Token[] commandArguments);
}
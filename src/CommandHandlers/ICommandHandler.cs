namespace Shell;

public interface ICommandHandler
{
    public string? HandleCommand(string[] commandArguments);
}
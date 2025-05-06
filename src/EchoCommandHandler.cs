namespace Shell;

public class EchoCommandHandler : ICommandHandler
{
    public string? HandleCommand(string commandContent)
    {
        return commandContent;
    }
}
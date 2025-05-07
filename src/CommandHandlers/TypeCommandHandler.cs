namespace Shell;

public class TypeCommandHandler : ICommandHandler
{
    public string? HandleCommand(string[] commandArguments)
    {
        var commandArgument = commandArguments[0];
        return CommandPrompt.Commands.ContainsKey(commandArgument) ? 
            $"{commandArgument} is a shell builtin" : 
            $"{commandArgument}: not found";
    }
}
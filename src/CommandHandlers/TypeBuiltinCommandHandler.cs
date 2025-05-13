namespace Shell.CommandHandlers;

public class TypeBuiltinCommandHandler : IBuiltinCommandHandler
{
    public string? HandleCommand(string[] commandArguments)
    {
        var commandArgument = commandArguments[0];
        
        if (CommandPrompt.Commands.ContainsKey(commandArgument))
        {
            return $"{commandArgument} is a shell builtin";
        }

        var programPath = PathHelper.GetProgramPath(commandArgument);

        return string.IsNullOrEmpty(programPath) ? 
            $"{commandArgument}: not found" : 
            $"{commandArgument} is {programPath}";
    }
}
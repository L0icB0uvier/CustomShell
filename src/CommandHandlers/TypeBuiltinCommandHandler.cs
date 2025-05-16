namespace Shell.CommandHandlers;

public class TypeBuiltinCommandHandler : IBuiltinCommandHandler
{
    public string? HandleCommand(Token[] commandArguments)
    {
        var commandArgument = commandArguments[0];
        
        if (CommandPrompt.Commands.ContainsKey(commandArgument.TokenValue))
        {
            return $"{commandArgument.TokenValue} is a shell builtin";
        }

        var programPath = PathHelper.GetProgramPath(commandArgument.TokenValue);

        return string.IsNullOrEmpty(programPath) ? 
            $"{commandArgument.TokenValue}: not found" : 
            $"{commandArgument.TokenValue} is {programPath}";
    }
}
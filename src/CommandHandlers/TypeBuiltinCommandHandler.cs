namespace Shell.CommandHandlers;

public class TypeBuiltinCommandHandler : IBuiltinCommandHandler
{
    public CommandOutput[]? HandleCommand(Token[] commandArguments)
    {
        var commandArgument = commandArguments[0];
        
        if (CommandPrompt.Commands.ContainsKey(commandArgument.TokenValue))
        {
            return [new CommandOutput($"{commandArgument.TokenValue} is a shell builtin", true)];
        }

        var programPath = PathHelper.GetProgramPath(commandArgument.TokenValue);

        return string.IsNullOrEmpty(programPath) ? 
            [new CommandOutput($"{commandArgument.TokenValue}: not found", true)] : 
            [new CommandOutput($"{commandArgument.TokenValue} is {programPath}")];
    }
}
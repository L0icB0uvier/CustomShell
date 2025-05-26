namespace Shell.CommandHandlers;

public class CdCommandHandler : IBuiltinCommandHandler
{
    public CommandOutput[]? HandleCommand(Token[] commandArguments)
    {
        var pathArgument = commandArguments[0];

        if (pathArgument.TokenValue == "~")
        {
            var homePath = Environment.GetEnvironmentVariable("HOME") ?? Environment.GetEnvironmentVariable("USERPROFILE");
            
            Directory.SetCurrentDirectory(homePath);
            return null;
        }
        
        if (Directory.Exists(pathArgument.TokenValue) == false)
        {
            return [new CommandOutput($"cd: {pathArgument.TokenValue}: No such file or directory", true)];
        }

        Directory.SetCurrentDirectory(pathArgument.TokenValue);
        return null;
    }
}
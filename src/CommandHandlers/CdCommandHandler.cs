namespace Shell.CommandHandlers;

public class CdCommandHandler : IBuiltinCommandHandler
{
    public string? HandleCommand(string[] commandArguments)
    {
        var pathArgument = commandArguments[0];

        if (pathArgument == "~")
        {
            var homePath = Environment.GetEnvironmentVariable("HOME") ?? Environment.GetEnvironmentVariable("USERPROFILE");
            
            Directory.SetCurrentDirectory(homePath);
            return null;
        }
        
        if (Directory.Exists(pathArgument) == false)
        {
            return $"cd: {pathArgument}: No such file or directory";
        }

        Directory.SetCurrentDirectory(pathArgument);
        return null;
    }
}
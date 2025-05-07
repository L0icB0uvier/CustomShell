namespace Shell;

public class TypeCommandHandler : ICommandHandler
{
    public string? HandleCommand(string[] commandArguments)
    {
        var commandArgument = commandArguments[0];
        
        if (CommandPrompt.Commands.ContainsKey(commandArgument))
        {
            return $"{commandArgument} is a shell builtin";
        } 
        
        var environmentPath = Environment.GetEnvironmentVariable("PATH");

        if (string.IsNullOrEmpty(environmentPath) == false)
        {
            var paths = environmentPath.Split(Path.PathSeparator);
            foreach (var path in paths)
            {
                var commandPath = Path.Combine(path, commandArgument);
                if (File.Exists(commandPath))
                {
                    return $"{commandArgument} is {commandPath}";
                }
            }
        }
        
        return $"{commandArgument}: not found";
    }
}
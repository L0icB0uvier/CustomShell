namespace Shell.CommandHandlers;

public class CdCommandHandler : IBuiltinCommandHandler
{
    public string? HandleCommand(string[] commandArguments)
    {
        var absolutePath = commandArguments[0];
        if (Directory.Exists(absolutePath) == false)
        {
            return $"cd: {absolutePath}: No such file or directory";
        }

        Environment.CurrentDirectory = absolutePath;
        return null;
    }
}
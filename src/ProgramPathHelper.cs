namespace Shell;

public class ProgramPathHelper
{
    public static string? GetProgramPath(string commandArgument)
    {
        var environmentPath = Environment.GetEnvironmentVariable("PATH");

        if (string.IsNullOrEmpty(environmentPath) == false)
        {
            var paths = environmentPath.Split(Path.PathSeparator);
            foreach (var path in paths)
            {
                var commandPath = Path.Combine(path, commandArgument);
                if (File.Exists(commandPath))
                {
                    return commandPath;
                }
            }
        }

        return null;
    }
}
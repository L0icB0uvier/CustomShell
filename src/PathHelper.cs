using System.Text.RegularExpressions;

namespace Shell;

public static class PathHelper
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
    
    public static bool IsLikelyFilePath(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        
        // Check if file exists
        if (File.Exists(input) || Directory.Exists(input))
            return true;

        // Check for typical path indicators (heuristics)
        bool hasDriveLetter = Regex.IsMatch(input, @"^[a-zA-Z]:[\\/]");
        bool hasSlashes = input.Contains("\\") || input.Contains("/");
        bool hasExtension = Path.HasExtension(input);

        // Return true if at least 2 of the 3 indicators are true
        int score = 0;
        if (hasDriveLetter) score++;
        if (hasSlashes) score++;
        if (hasExtension) score++;

        return score >= 1;
    }
}
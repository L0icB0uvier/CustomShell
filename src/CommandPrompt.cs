using System.Diagnostics;
using System.Text.RegularExpressions;
using Shell.CommandHandlers;

namespace Shell;

public class CommandPrompt
{
    public static Dictionary<string, IBuiltinCommandHandler> Commands { get; } = new Dictionary<string, IBuiltinCommandHandler>();

    public static void AddCommand(string command, IBuiltinCommandHandler handler)
    {
        if (!Commands.TryAdd(command, handler))
        {
            Console.WriteLine($"{command}: command already exists");
        }
    }

    public void Start()
    {
        while (true)
        {
            Console.Write("$ ");
            
            //Read command
            var command = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(command)) continue;
            
            //Split command
            var commandParts = ParseCommand(command);
            var commandName = commandParts.First();
            var arguments = commandParts.Skip(1).ToArray();
            
            //Process command
            if(TryProcessBuiltinCommand(commandName, arguments)) continue;
            if(TryProcessExternalProgram(commandName, arguments)) continue;
            
            //Command not found
            PrintCommandNotFound(commandName);
        }
    }
    
    private string[] ParseCommand(string command)
    {
        string pattern = @"[^\s""']+|'([^']*)'|""([^""]*)""";
        var matches = Regex.Matches(command, pattern);
       
        var tokens = ProcessMatches(matches);

        return tokens;
    }
    
    private static string[] ProcessMatches(MatchCollection matches)
    {
        List<string> tokens = new List<string>();
        string currentToken = String.Empty;
        int currentGroupIndex = 0;
        
        for (int i = 0; i < matches.Count; i++)
        {
            Match current = matches[i];

            int groupIndex = current.Groups[2].Success ? 2 : current.Groups[1].Success ? 1 : 0;
            
            string value = current.Groups[groupIndex].Value;
            
            if (i == 0)
            {
                currentToken = value;
                continue;
            }
            
            Match previous = matches[i - 1];
            int prevEnd = previous.Index + previous.Length;
            int currStart = current.Index;

            bool isAdjacent = prevEnd == currStart;

            if (groupIndex == currentGroupIndex && isAdjacent)
            {
                currentToken += value;
            }

            else
            {
                tokens.Add(currentToken);
                currentToken = value;
                currentGroupIndex = groupIndex;
            }
        }

        if (!string.IsNullOrWhiteSpace(currentToken))
        {
            tokens.Add(currentToken);
        }

        return tokens.ToArray();
    }


    private static bool TryProcessBuiltinCommand(string command, string[] arguments)
    {
        if (!Commands.TryGetValue(command, out IBuiltinCommandHandler? handler)) return false;
        
        var commandMessage = handler.HandleCommand(arguments);
        if (commandMessage != null) Console.WriteLine(commandMessage);
        
        return true;
    }

    private static bool TryProcessExternalProgram(string command, string[] arguments)
    {
        var programPath = ProgramPathHelper.GetProgramPath(command);
        if (string.IsNullOrEmpty(programPath)) return false;
        
        //Add double quotes around arguments they are paths
        string[] correctedArguments = new string[arguments.Length];

        for (int i = 0; i < arguments.Length; i++)
        {
            correctedArguments[i] = IsLikelyFilePath(arguments[i])? $"\"{arguments[i]}\"" : arguments[i];
        }
        
        var processStartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = string.Join(' ', correctedArguments),
            WorkingDirectory = Path.GetDirectoryName(programPath)
        };
        
        var process = Process.Start(processStartInfo);
        process?.WaitForExit();
        return true;
    }

    private static void PrintCommandNotFound(string command)
    {
        Console.WriteLine($"{command}: command not found");
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

        return score >= 2;
    }
}
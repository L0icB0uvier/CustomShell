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
            var commandParts = CommandParser.ParseCommand(command);
            var commandName = commandParts.First();
            var arguments = commandParts.Skip(1).ToArray();
            
            //Process command
            if(TryProcessBuiltinCommand(commandName, arguments)) continue;
            if(TryProcessExternalProgram(commandName, arguments)) continue;
            
            //Command not found
            PrintCommandNotFound(commandName);
        }
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
        var programPath = PathHelper.GetProgramPath(command);
        if (string.IsNullOrEmpty(programPath)) return false;
        
        //Add double quotes around arguments they are paths
        string[] correctedArguments = new string[arguments.Length];

        for (int i = 0; i < arguments.Length; i++)
        {
            correctedArguments[i] = PathHelper.IsLikelyFilePath(arguments[i])? $"\"{arguments[i]}\"" : arguments[i];
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
}
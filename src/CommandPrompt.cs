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
    
    private static bool TryProcessBuiltinCommand(Token command, Token[] arguments)
    {
        if (!Commands.TryGetValue(command.TokenValue, out IBuiltinCommandHandler? handler)) return false;
        
        var commandMessage = handler.HandleCommand(arguments);
        if (commandMessage != null) Console.WriteLine(commandMessage);
        
        return true;
    }

    private static bool TryProcessExternalProgram(Token command, Token[] arguments)
    {
        var programPath = PathHelper.GetProgramPath(command.TokenValue);
        if (string.IsNullOrEmpty(programPath)) return false;
        
        //Add double quotes around arguments they are paths
        string[] correctedArguments = new string[arguments.Length];

        for (int i = 0; i < arguments.Length; i++)
        {
            string correctedValue;
            if (arguments[i].TokenValue.Contains("\""))
            {
                correctedValue = $"{arguments[i].TokenValue}";
            }
            else
            {
                correctedValue = PathHelper.IsLikelyFilePath(arguments[i].TokenValue)
                    ? $"\"{arguments[i].TokenValue}\""
                    : arguments[i].TokenValue;
            }
            correctedArguments[i] = correctedValue;
        }
        
        string argumentsString = string.Join(' ', correctedArguments);
        
        var processStartInfo = new ProcessStartInfo
        {
            FileName = command.TokenValue,
            WorkingDirectory = Path.GetDirectoryName(programPath)
        };
        
        foreach (var correctedArgument in arguments)
        {
            processStartInfo.ArgumentList.Add(correctedArgument.TokenValue);
        }
        
        var process = Process.Start(processStartInfo);
        process?.WaitForExit();
        return true;
    }

    private static void PrintCommandNotFound(Token command){
        Console.WriteLine($"{command.TokenValue}: command not found");
    }
}
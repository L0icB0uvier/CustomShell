using System.Diagnostics;
using Shell.CommandHandlers;
using Shell.Tokens;

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

            if (arguments.Length > 0 && arguments[^1].TokenType == TokenType.Redirect)
            {
                var redirectToken = (RedirectToken)arguments[^1];
                using (var writer = new StreamWriter(redirectToken.RedirectPath))
                {
                    TextWriter originalOut;
                    switch (redirectToken.RedirectType)
                    {
                        case RedirectType.StandardError:
                            originalOut = Console.Error;
                            Console.SetError(writer);
                            break;
                        case RedirectType.StandardOutput:
                        default:
                            originalOut = Console.Out;
                            Console.SetOut(writer);
                            break;
                    }

                    arguments = arguments.Where(a => a.TokenType != TokenType.Redirect).ToArray();
                    
                    ProcessCommand(commandName, arguments);
                    
                    switch (redirectToken.RedirectType)
                    {
                        case RedirectType.StandardError:
                            Console.SetError(originalOut);
                            break;
                        case RedirectType.StandardOutput:
                        default:
                            Console.SetOut(originalOut);
                            break;
                    }
                }
            }

            else
            {
                ProcessCommand(commandName, arguments);;
            }
        }
    }

    private static void ProcessCommand(Token commandName, Token[] arguments)
    {
        //Process command
        if(TryProcessBuiltinCommand(commandName, arguments)) return;
        if(TryProcessExternalProgram(commandName, arguments)) return;
                    
        //Command not found
        PrintCommandNotFound(commandName);
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
        
        if (correctedArguments == null) throw new ArgumentNullException(nameof(correctedArguments));

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
        
        
        var processStartInfo = new ProcessStartInfo
        {
            FileName = command.TokenValue,
            WorkingDirectory = Path.GetDirectoryName(programPath),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        
        foreach (var correctedArgument in arguments)
        {
            processStartInfo.ArgumentList.Add(correctedArgument.TokenValue);
        }
        
        var process = Process.Start(processStartInfo);
        string? output = process?.StandardOutput.ReadToEnd();
        string? error = process?.StandardError.ReadToEnd();
        
        process?.WaitForExit();
        
        if (string.IsNullOrEmpty(output) == false)
        {
            if (output.EndsWith(Environment.NewLine))
            {
                Console.Write(output);
            }
            else
            {
                Console.WriteLine(output);       
            }
        }

        if (string.IsNullOrEmpty(error) == false)
        {
            if (error.EndsWith(Environment.NewLine))
            {
                Console.Error.Write(error);
            }
            else
            {
                Console.Error.WriteLine(error);       
            }
        }
        
        return true;
    }

    private static void PrintCommandNotFound(Token command){
        Console.WriteLine($"{command.TokenValue}: command not found");
    }
}
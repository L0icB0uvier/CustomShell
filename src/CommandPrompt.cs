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
                var appendRedirect = redirectToken.RedirectType 
                    is RedirectType.AppendOutput 
                    or RedirectType.AppendError;
                
                using (var redirectWriter = new StreamWriter(redirectToken.RedirectPath, appendRedirect))
                {
                    var redirectError =
                        redirectToken.RedirectType is RedirectType.StandardError or RedirectType.AppendError;
                    
                    TextWriter originalWriter = redirectError ? Console.Error : Console.Out;

                    RedirectOutput(redirectWriter, redirectError);
                    
                    arguments = arguments.Where(a => a.TokenType != TokenType.Redirect).ToArray();
                    
                    var commandOutputs = ProcessCommand(commandName, arguments);
                    
                    PrintOutput(commandOutputs);
                    
                    RedirectOutput(originalWriter, redirectError);
                }
            }

            else
            {
                var output = ProcessCommand(commandName, arguments);
                PrintOutput(output);
            }
        }
    }

    private static void RedirectOutput(TextWriter writer, bool redirectError)
    {
        if (redirectError)
        {
            Console.SetError(writer);
        }

        else
        {
            Console.SetOut(writer);
        }
    }

    private static CommandOutput[]? ProcessCommand(Token commandName, Token[] arguments)
    {
        //Process command
        if(TryProcessBuiltinCommand(commandName, arguments, out var builtinOutput)) return builtinOutput;
        if(TryProcessExternalProgram(commandName, arguments,out var externalProgramOutput)) return externalProgramOutput;
                    
        //Command not found
        return [PrintCommandNotFound(commandName)];
    }
    
    private static bool TryProcessBuiltinCommand(Token command, Token[] arguments, out CommandOutput[]? output)
    {
        if (!Commands.TryGetValue(command.TokenValue, out IBuiltinCommandHandler? handler))
        {
            output = null;
            return false;
        }
        
        output = handler.HandleCommand(arguments);
        return true;
    }

    private static bool TryProcessExternalProgram(Token command, Token[] arguments, out CommandOutput[]? output)
    {
        var programPath = PathHelper.GetProgramPath(command.TokenValue);
        
        if (string.IsNullOrEmpty(programPath))
        {
            output = null;
            return false;
        }
        
        var processStartInfo = new ProcessStartInfo
        {
            FileName = command.TokenValue,
            WorkingDirectory = Path.GetDirectoryName(programPath),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        
        foreach (var argumentToken in arguments)
        {
            processStartInfo.ArgumentList.Add(argumentToken.TokenValue);
        }
        
        var process = Process.Start(processStartInfo);
        
        
        process?.WaitForExit();
        
        string? standardOutput = process?.StandardOutput.ReadToEnd();
        string? errorOutput = process?.StandardError.ReadToEnd();

        var commandOutputs = new List<CommandOutput>();

        if (string.IsNullOrEmpty(standardOutput) == false)
        {
            commandOutputs.Add(new CommandOutput(standardOutput));
        }

        if (string.IsNullOrEmpty(errorOutput) == false)
        {
            commandOutputs.Add(new CommandOutput(errorOutput, true));       
        }
        output = commandOutputs.Count > 0 ? commandOutputs.ToArray() : null;
        
        return true;
    }

    private static CommandOutput PrintCommandNotFound(Token command){
       return new CommandOutput($"{command.TokenValue}: command not found",true);
    }

    private static void PrintOutput(CommandOutput[]? commandOutputs)
    {
        if(commandOutputs == null) return;
        
        foreach (var output in commandOutputs)
        {
            if (output.OutputText.EndsWith(Environment.NewLine))
            {
                if (output.IsError)
                {
                    Console.Error.Write(output.OutputText);
                }

                else
                {
                    Console.Write(output.OutputText);
                }
            }

            else
            {
                if (output.IsError)
                {
                    Console.Error.WriteLine(output.OutputText);
                }

                else
                {
                    Console.WriteLine(output.OutputText);
                }
            }
        }

        
    }
}
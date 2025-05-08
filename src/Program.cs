using Shell.CommandHandlers;

namespace Shell;

public class Program
{
    public static void Main(string[] args)
    {
        CommandPrompt prompt = new CommandPrompt();
        
        CommandPrompt.AddCommand("echo", new EchoBuiltinCommandHandler());
        CommandPrompt.AddCommand("exit", new ExitBuiltinCommandHandler());
        CommandPrompt.AddCommand("type", new TypeBuiltinCommandHandler());
        CommandPrompt.AddCommand("pwd", new PwdCommandHandler());
        
        prompt.Start();
    }
}


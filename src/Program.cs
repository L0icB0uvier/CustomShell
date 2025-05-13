using Shell.CommandHandlers;

namespace Shell;

public class Program
{
    public static void Main(string[] args)
    {
        /*RegexTesting regexTesting = new RegexTesting();
        regexTesting.Start();*/
        
        CommandPrompt prompt = new CommandPrompt();
        
        CommandPrompt.AddCommand("echo", new EchoBuiltinCommandHandler());
        CommandPrompt.AddCommand("exit", new ExitBuiltinCommandHandler());
        CommandPrompt.AddCommand("type", new TypeBuiltinCommandHandler());
        CommandPrompt.AddCommand("pwd", new PwdCommandHandler());
        CommandPrompt.AddCommand("cd", new CdCommandHandler());
        
        prompt.Start();
    }
}


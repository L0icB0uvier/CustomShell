using Shell.CommandHandlers;

namespace Shell;

public static class Program
{
    public static void Main(string[] args)
    {
        //StartRegexTesting();
        StartCommandPrompt();
    }

    private static void StartRegexTesting()
    {
        RegexTesting regexTesting = new RegexTesting();
        regexTesting.Start();
    }

    private static void StartCommandPrompt()
    {
        CommandPrompt prompt = new CommandPrompt();
        
        CommandPrompt.AddCommand("echo", new EchoBuiltinCommandHandler());
        CommandPrompt.AddCommand("exit", new ExitBuiltinCommandHandler());
        CommandPrompt.AddCommand("type", new TypeBuiltinCommandHandler());
        CommandPrompt.AddCommand("pwd", new PwdCommandHandler());
        CommandPrompt.AddCommand("cd", new CdCommandHandler());
        
        prompt.Start();
    }
}


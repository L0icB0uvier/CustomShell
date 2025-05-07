namespace Shell;

public class Program
{
    public static void Main(string[] args)
    {
        CommandPrompt prompt = new CommandPrompt();
        prompt.AddCommand("echo", new EchoCommandHandler());
        prompt.AddCommand("exit", new ExitCommandHandler());
        prompt.AddCommand("type", new TypeCommandHandler());
        prompt.Start();
    }
}


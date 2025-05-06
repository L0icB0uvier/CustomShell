namespace Shell;

public class CommandPrompt
{
    private Dictionary<string, ICommandHandler> commands = new Dictionary<string, ICommandHandler>();

    public void AddCommand(string command, ICommandHandler handler)
    {
        if (!commands.TryAdd(command, handler))
        {
            Console.WriteLine($"{command}: command already exists");
        }
    }

    public void Start()
    {
        while (true)
        {
            Console.Write("$ ");
            
            var command = Console.ReadLine();

            if (command == "exit 0")
            {
                Environment.Exit(0);
                return;
            }

            var commandParts = command.Split(' ');
            var commandName = commandParts[0];
            
            string commandContent = String.Empty;
            if (commandParts.Length >= 2)
            {
                commandContent = commandParts[1];
            }

            if (commands.TryGetValue(commandName, out ICommandHandler? handler))
            {
                var commandMessage = handler.HandleCommand(commandContent);
                Console.WriteLine(commandMessage);
            }

            else
            {
                Console.WriteLine($"{command}: command not found");
            }
        }
    }
}
namespace Shell;

public class CommandPrompt
{
    public static Dictionary<string, ICommandHandler> Commands { get; } = new Dictionary<string, ICommandHandler>();

    public void AddCommand(string command, ICommandHandler handler)
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
            
            var command = Console.ReadLine();

            if (command == null || string.IsNullOrWhiteSpace(command))
            {
                continue;
            }
            
            var commandParts = command.Split(' ');
            var commandName = commandParts[0];
            
            if (Commands.TryGetValue(commandName, out ICommandHandler? handler))
            {
                var commandMessage = handler.HandleCommand(commandParts.Skip(1).ToArray());

                if (commandMessage != null)
                {
                    Console.WriteLine(commandMessage);
                }
            }

            else
            {
                Console.WriteLine($"{command}: command not found");
            }
        }
    }
}
using System.Net;
using System.Net.Sockets;

while (true)
{
    Console.Write("$ ");
    
    var command = Console.ReadLine();

    if (command == "exit")
    {
        Environment.Exit(0);
    }
    Console.WriteLine($"{command}: command not found");
}
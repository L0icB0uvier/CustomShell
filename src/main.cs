using System.Net;
using System.Net.Sockets;

while (true)
{
    Console.Write("$ ");
    
    var command = Console.ReadLine();

    Console.WriteLine($"{command}: command not found");
    Environment.Exit(0);
    break;
}
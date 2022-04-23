using RapidUDP;
using System.Text.Json;


string? ReceiveMessage(Package p)
{
    Console.WriteLine("Client sent {0}", p.Text);

    return JsonSerializer.Serialize(new State("Elite", 1337));
}


var server = new Server("0.0.0.0", 8080);
var f = ReceiveMessage;

server.AddCallback(new UDPCallback(ReceiveMessage));

server.Start();

record State(string name, int value);
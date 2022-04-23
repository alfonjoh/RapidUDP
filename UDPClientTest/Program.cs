using RapidUDP;
using System.Diagnostics;

string InitialCallback()
{
    return "Hello";
}


int counter = 0;
string? ReceiveMessage(Package<State> p)
{
    if ((counter++) > 10) return null;
    var json = p.Json;
    if (json != null)
    {
        Console.WriteLine("Server responded with: {0}", json.name);
    }

    return $"Sup {counter}";
}

var server = new Client("127.0.0.1", 8080);

server.AddInitialCallback(new InitialUDPCallback(InitialCallback));
server.AddCallback(new UDPCallback<State>(ReceiveMessage));

Stopwatch st = new Stopwatch();
st.Start();
server.Start();
st.Stop();
Console.WriteLine("Took {0} ms", st.ElapsedMilliseconds);

record State(string name, int value);

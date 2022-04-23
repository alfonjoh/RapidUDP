using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RapidUDP
{
    public class Client
    {
        private readonly string Host;
        private readonly int Port;
        private UdpClient? client;
        private List<UDPCallback> Callbacks = new List<UDPCallback>();
        private List<InitialUDPCallback> InitialCallbacks = new List<InitialUDPCallback>();


        public Client(string host, int port)
        {
            Host = host;
            Port = port;
            client = null;
        }

        public void AddCallback(UDPCallback callback)
        {
            Callbacks.Add(callback);
        }

        public void AddInitialCallback(InitialUDPCallback callback)
        {
            InitialCallbacks.Add(callback);
        }

        private byte[] GetInitialMessage()
        {
            byte[]? output = null;
            foreach (var callback in InitialCallbacks)
            {
                output = callback.Callback();
                if (output != null) break;
            }
            if (output == null) throw new UDPCallBackException("No initial callback returned a valid response");
            return output;
        }


        public void Start()
        {
            client = new UdpClient();

            IPAddress addr = IPAddress.Parse(Host);
            IPEndPoint remote = new IPEndPoint(addr, Port);
            client.Connect(remote);

            byte[]? message = GetInitialMessage();

            while (true)
            {
                client.Send(message);
                var response = client.Receive(ref remote);
                message = MakeCallback(ref response);
                if (message == null) break;
            }
        }

        private byte[]? MakeCallback(ref byte[] bytes)
        {
            byte[]? buffer = null;
            foreach (var callback in Callbacks)
            {
                buffer = callback.Callback(bytes);
                if (buffer != null) break;
            }

            return buffer;
        }
    }
}
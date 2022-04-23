using System.Net;
using System.Net.Sockets;

namespace RapidUDP
{
    public class Server
    {
        private readonly string Host;
        private readonly int Port;
        private UdpClient? Client;
        private List<IUDPCallback> callbacks = new List<IUDPCallback>();


        public Server(string host, int port)
        {
            Host = host;
            Port = port;
            Client = null;
        }

        public void AddCallback(IUDPCallback action)
        {
            callbacks.Add(action);
        }


        public void Start()
        {
            if (Host == "0.0.0.0")
            {
                Client = new UdpClient(Port);
            }
            else
            {
                Client = new UdpClient(Host, Port);
            }
            var remoteEP = new IPEndPoint(IPAddress.Any, Port);

            try
            {
                while (true)
                {
                    byte[] recBytes = Client.Receive(ref remoteEP);
                    byte[]? sendBytes = MakeCallback(ref recBytes);
                    if (sendBytes != null)
                    {
                        Client.Send(sendBytes, remoteEP);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Client.Close();
            }
        }

        private byte[]? MakeCallback(ref byte[] bytes)
        {
            byte[]? buffer = null;
            foreach (var callback in callbacks)
            {
                buffer = callback.Callback(bytes);
                if (buffer != null) break;
            }

            return buffer;
        }
    }
}
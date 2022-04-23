using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RapidUDP
{
    public class Server
    {
        private string Host;
        private int Port;
        private UdpClient Client;
        private List<UDPCallback> callbacks = new List<UDPCallback>();


        public Server(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public void AddCallback(UDPCallback action)
        {
            callbacks.Add(action);
        }


        public void Start()
        {
            Client = new UdpClient(Host, Port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, Port);

            try
            {
                while (true)
                {
                    byte[] recBytes = Client.Receive(ref groupEP);
                    byte[]? sendBytes = MakeCallback(ref recBytes);
                    if (sendBytes != null)
                    {
                        Client.Send(sendBytes);
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
                if (buffer != null)
                {
                    buffer = callback.Callback(bytes);
                }
                else
                {
                    callback.Callback(bytes);
                }
            }

            return buffer;
        }
    }

    public class UDPCallback
    {
        object CallbackFunc;

        public UDPCallback(Action<byte[]> action) { CallbackFunc = action; }
        public UDPCallback(Action<string> action) { CallbackFunc = action; }
        public UDPCallback(Func<byte[]> action) { CallbackFunc = action; }
        public UDPCallback(Func<string> action) { CallbackFunc = action; }
        public UDPCallback(Func<byte[], byte[]> action) { CallbackFunc = action; }
        public UDPCallback(Func<byte[], string> action) { CallbackFunc = action; }
        public UDPCallback(Func<string, byte[]> action) { CallbackFunc = action; }
        public UDPCallback(Func<string, string> action) { CallbackFunc = action; }

        internal byte[]? Callback(byte[] buff)
        {
            if (CallbackFunc is Action<byte[]>)
                ((Action<byte[]>)CallbackFunc)(buff);
            else if (CallbackFunc is Action<string>)
                ((Action<string>)CallbackFunc)(Encoding.UTF8.GetString(buff));
            else if (CallbackFunc is Func<byte[]>)
                return ((Func<byte[]>)CallbackFunc)();
            else if (CallbackFunc is Func<string>)
                return Encoding.UTF8.GetBytes(((Func<string>)CallbackFunc)());
            else if (CallbackFunc is Func<byte[], string>)
                return Encoding.UTF8.GetBytes(((Func<byte[], string>)CallbackFunc)(buff));
            else if (CallbackFunc is Func<string, string>)
                return Encoding.UTF8.GetBytes(((Func<string, string>)CallbackFunc)(Encoding.UTF8.GetString(buff)));
            else if (CallbackFunc is Func<byte[], byte[]>)
                return ((Func<byte[], byte[]>)CallbackFunc)(buff);
            else if (CallbackFunc is Func<string, byte[]>)
                return ((Func<string, byte[]>)CallbackFunc)(Encoding.UTF8.GetString(buff));

            return null;
        }
    }

    public class ServerResponseCallback
    {
        public ServerResponseCallback(ref byte[] bytes)
        {
            Bytes = bytes;
        }

        public byte[] Bytes { get; }
        public string Text
        {
            get
            {
                return Encoding.UTF8.GetString(Bytes);
            }
        }
    }
}
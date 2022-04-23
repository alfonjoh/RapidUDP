using System.Net;
using System.Text;

namespace RapidUDP
{
    public class Server
    {
        private IPAddress Address;
        private int Port;

        public Server(string addr, int port)
        {
            var byteAddr = Encoding.ASCII.GetBytes(addr);
            Address = new IPAddress(byteAddr);
            Port = port;
        }
    }
}
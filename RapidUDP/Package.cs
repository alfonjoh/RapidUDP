using System.Text;
using System.Text.Json;

namespace RapidUDP
{
    public class Package<T>
    {
        private byte[] data;
        public byte[] Data { get { return data; } }
        public string Text { get { return Encoding.UTF8.GetString(data); } }
        public T? Json
        {
            get
            {
                return JsonSerializer.Deserialize<T>(data);
            }
        }

        internal Package(ref byte[] buff)
        {
            data = buff;
        }

        public Package()
        {
            throw new NotImplementedException("Cannot create new package as user");
        }
    }

    public class Package : Package<Dictionary<string, object>>
    {
        public Package(ref byte[] buff) : base(ref buff)
        {
        }
    }
}

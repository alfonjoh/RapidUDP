using System.Text;

namespace RapidUDP
{
    public class UDPCallback
    {
        object CallbackFunc;

        public UDPCallback(Action<byte[]> action) { CallbackFunc = action; }
        public UDPCallback(Action<string> action) { CallbackFunc = action; }
        public UDPCallback(Func<byte[]?> action) { CallbackFunc = action; }
        public UDPCallback(Func<string?> action) { CallbackFunc = action; }
        public UDPCallback(Func<byte[], byte[]?> action) { CallbackFunc = action; }
        public UDPCallback(Func<byte[], string?> action) { CallbackFunc = action; }
        public UDPCallback(Func<string, byte[]?> action) { CallbackFunc = action; }
        public UDPCallback(Func<string, string?> action) { CallbackFunc = action; }

        internal byte[]? Callback(byte[] buff)
        {
            object? output = null;
            if (CallbackFunc is Action<byte[]>)
                ((Action<byte[]>)CallbackFunc)(buff);
            else if (CallbackFunc is Action<string>)
                ((Action<string>)CallbackFunc)(Encoding.UTF8.GetString(buff));
            else if (CallbackFunc is Func<byte[]?>)
                output = ((Func<byte[]?>)CallbackFunc)();
            else if (CallbackFunc is Func<string?>)
                output = ((Func<string?>)CallbackFunc)();
            else if (CallbackFunc is Func<byte[], string?>)
                output = ((Func<byte[], string?>)CallbackFunc)(buff);
            else if (CallbackFunc is Func<string, string?>)
                output = ((Func<string, string?>)CallbackFunc)(Encoding.UTF8.GetString(buff));
            else if (CallbackFunc is Func<byte[], byte[]?>)
                output = ((Func<byte[], byte[]?>)CallbackFunc)(buff);
            else if (CallbackFunc is Func<string, byte[]?>)
                output = ((Func<string, byte[]?>)CallbackFunc)(Encoding.UTF8.GetString(buff));

            if (output == null) return null;
            if (output is byte[])
            {
                return (byte[])output;
            }
            if (output is string)
            {
                return Encoding.UTF8.GetBytes((string)output);
            }
            return null;
        }
    }

    public class InitialUDPCallback
    {
        object CallbackFunc;

        public InitialUDPCallback(Func<byte[]?> action) { CallbackFunc = action; }
        public InitialUDPCallback(Func<string?> action) { CallbackFunc = action; }

        internal byte[]? Callback()
        {
            object? output = null;
            if (CallbackFunc is Func<byte?[]>)
                output = ((Func<byte[]?>)CallbackFunc)();
            else if (CallbackFunc is Func<string?>)
                output = ((Func<string?>)CallbackFunc)();

            if (output == null) return null;
            if (output is byte[])
            {
                return (byte[])output;
            }
            if (output is string)
            {
                return Encoding.UTF8.GetBytes((string)output);
            }
            return null;
        }
    }

    public class UDPCallBackException : Exception {
        public UDPCallBackException()
        {
        }

        public UDPCallBackException(string message)
            : base(message)
        {
        }

        public UDPCallBackException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
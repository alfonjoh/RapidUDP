using System.Text;

namespace RapidUDP
{
    public interface IUDPCallback
    {
        internal byte[]? Callback(byte[] buff);
    }

    public class UDPCallback<T> : IUDPCallback where T : class
    {
        protected object CallbackFunc;

        public UDPCallback(Action<Package<T>> action) { CallbackFunc = action; }

        public UDPCallback(Func<object?> action) { CallbackFunc = action; }

        public UDPCallback(Func<Package<T>, object?> action) { CallbackFunc = action; }

        byte[]? IUDPCallback.Callback(byte[] buff)
        {
            object? output = null;

            if (CallbackFunc is Action<Package<T>>)
                ((Action<Package<T>>)CallbackFunc)(new Package<T>(ref buff));
            else if (CallbackFunc is Func<object?>)
                output = ((Func<object?>)CallbackFunc)();
            else if (CallbackFunc is Func<Package<T>, object?>)
                output = ((Func<Package<T>, object?>)CallbackFunc)(new Package<T>(ref buff));

            if (output == null) return null;
            if (output is byte[])
                return (byte[])output;
            else if (output is string)
                return Encoding.UTF8.GetBytes((string)output);

            throw new UDPCallBackException("The callback function is not valid");
        }
    }

    public class UDPCallback : IUDPCallback
    {
        protected object CallbackFunc;

        public UDPCallback(Action<Package> action) { CallbackFunc = action; }

        public UDPCallback(Func<object?> action) { CallbackFunc = action; }

        public UDPCallback(Func<Package, object?> action) { CallbackFunc = action; }

        byte[]? IUDPCallback.Callback(byte[] buff)
        {
            object? output = null;

            if (CallbackFunc is Action<Package>)
                ((Action<Package>)CallbackFunc)(new Package(ref buff));
            else if (CallbackFunc is Func<object?>)
                output = ((Func<object?>)CallbackFunc)();
            else if (CallbackFunc is Func<Package, object?>)
                output = ((Func<Package, object?>)CallbackFunc)(new Package(ref buff));

            if (output == null) return null;
            if (output is byte[])
                return (byte[])output;
            else if (output is string)
                return Encoding.UTF8.GetBytes((string)output);

            throw new UDPCallBackException("The callback function is not valid");
        }
    }

    public class InitialUDPCallback
    {
        object CallbackFunc;

        public InitialUDPCallback(Func<object?> action) { CallbackFunc = action; }

        internal byte[]? Callback()
        {
            object? output = null;
            if (CallbackFunc is Func<object?>)
                output = ((Func<object?>)CallbackFunc)();

            if (output == null) return null;
            if (output is byte[])
                return (byte[])output;
            if (output is string)
                return Encoding.UTF8.GetBytes((string)output);

            throw new UDPCallBackException("The callback function is not valid");
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
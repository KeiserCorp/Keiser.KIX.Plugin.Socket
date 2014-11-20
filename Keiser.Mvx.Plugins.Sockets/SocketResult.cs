namespace Keiser.Mvx.Plugins.Sockets
{
    using System;

    public class SocketResult : IAsyncResult
    {
        
        byte[] _result;

        public SocketResult(byte[] result)
        {
            _result = result;
        }

        public bool IsCompleted
        {
            get { return true; }
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public object AsyncState
        {
            get { return _result; }
        }

        public bool CompletedSynchronously
        {
            get { return true; }
        }
    }
}

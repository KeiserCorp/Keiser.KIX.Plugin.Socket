namespace Keiser.Plugin.Socket
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public class NetSocket : ISocket
    {
        public object Locker { get; set; }
        private Socket _socket;
        private EndPoint _endPointSender;
        private AsyncCallback _callback;
        private string _ipAddress;
        private int _port;

        private byte[] _resultBuffer;
        private byte[] _receiverBuffer = new byte[1024];

        private bool _keepRunning;
        private object _runningLocker = new object();


        public NetSocket()
        {
            Locker = new object();
        }

        public virtual void CreateBroadcastSender(string ipAddress, int port)
        {
            lock (Locker)
            {
                _ipAddress = ipAddress;
                _port = port;
            }
            CreateBroadcastSocket();
        }

        public virtual void Send(byte[] data)
        {
            if (_socket == null)
                return;
            try
            {
                _socket.SendTo(data, _endPointSender);
            }
            catch (Exception e)
            {
                Trace.Error("Socket Exception", e);
            }
        }

        public virtual void CreateListener(string ipAddress, int port)
        {
            lock (Locker)
            {
                _ipAddress = ipAddress;
                _port = port;
            }
            if (Running())
            {
                StopListener();
                StartListener(_callback);
            }
        }

        protected virtual void CreateListeningSocket()
        {
            lock (Locker)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, _port);
                _socket.Bind(ipEndPoint);
                IPAddress ipAddress = IPAddress.Parse(_ipAddress);
                MulticastOption multicastOption = new MulticastOption(ipAddress);
                _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
                _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
                _endPointSender = new IPEndPoint(IPAddress.Any, 0);
            }
        }

        protected virtual void CreateBroadcastSocket()
        {
            lock (Locker)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                _endPointSender = new IPEndPoint(IPAddress.Parse(_ipAddress), _port);
            }
        }

        public virtual bool StartListener(AsyncCallback callback = null)
        {
            if (Running())
                return false;
            CreateListeningSocket();
            lock (Locker)
            {
                _callback = callback;
            }
            lock (_runningLocker)
            {
                _keepRunning = true;
            }
            SocketListen();

            return true;
        }

        public virtual bool StopListener()
        {
            lock (_runningLocker)
            {
                _keepRunning = false;
            }
            _socket.Close();

            return true;
        }

        public virtual bool Running()
        {
            lock (_runningLocker)
            {
                return _keepRunning;
            }
        }

        protected virtual void SocketListen()
        {
            try
            {
                _socket.BeginReceiveFrom(_receiverBuffer, 0, _receiverBuffer.Length, SocketFlags.None, ref _endPointSender, ReceivedResult, _socket);
            }
            catch (Exception e)
            {
                Trace.Error("Socket Exception", e);
            }

        }

        protected virtual void ReceivedResult(IAsyncResult result)
        {
            int messageLength = 0;
            Socket receivedSocket = (Socket)result.AsyncState;
            try
            {
                messageLength = receivedSocket.EndReceive(result);
            }
            catch (Exception e)
            {
                Trace.Warn("Receiver Error {0}", e.ToString());
            }
            if (messageLength > 0)
            {
                _resultBuffer = new byte[messageLength];
                Array.Copy(_receiverBuffer, _resultBuffer, messageLength);
            }
            if (!Running())
                return;
            SocketListen();
            if (_callback != null && messageLength > 0)
            {
                _callback.Invoke(new SocketResult(_resultBuffer));
            }
        }

    }

}

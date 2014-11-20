namespace Keiser.MvxPlugins.Sockets
{
    using System;

    public interface ISocket
    {
        object Locker { get; }
        void CreateListener(string ipAddress, int port);
        bool StartListener(AsyncCallback callback);
        bool StopListener();
        bool Running { get; }
        void CreateBroadcastSender(string ipAddress, int port);
        void Send(byte[] data);
    }
}

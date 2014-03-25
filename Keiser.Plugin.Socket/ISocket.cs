﻿namespace Keiser.Plugin.Socket
{
    using System;

    public interface ISocket
    {
        object Locker { get; set; }
        void CreateListener(string ipAddress, int port);
        bool StartListener(AsyncCallback callback);
        bool StopListener();
        bool Running();
        void CreateBroadcastSender(string ipAddress, int port);
        void Send(byte[] data);
    }
}

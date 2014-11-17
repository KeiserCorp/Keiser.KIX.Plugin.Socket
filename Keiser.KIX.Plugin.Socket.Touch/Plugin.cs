namespace Keiser.KIX.Plugin.Socket.Touch
{
    using Cirrious.CrossCore;
    using Cirrious.CrossCore.Plugins;

    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<ISocket, TouchSocket>();
        }
    }
}

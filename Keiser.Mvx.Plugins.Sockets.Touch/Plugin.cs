namespace Keiser.Mvx.Plugins.Sockets.Touch
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

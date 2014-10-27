namespace Keiser.KIX.Plugin.Socket.iOS
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

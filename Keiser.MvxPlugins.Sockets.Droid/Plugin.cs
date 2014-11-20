namespace Keiser.MvxPlugins.Sockets.Droid
{
    using Cirrious.CrossCore;
    using Cirrious.CrossCore.Plugins;

    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<ISocket, DroidSocket>();
        }
    }
}

namespace Keiser.Mvx.Plugins.Sockets
{
    using System;
    using System.Diagnostics;
    using Cirrious.CrossCore.Platform;

    public class Trace
    {
        private const string Tag = "Keiser.Mvx.Plugins.Sockets";

        public static void Info(string message, params object[] args)
        {
            MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, Tag, message, args);
        }

        public static void Warn(string message, params object[] args)
        {
            MvxTrace.TaggedTrace(MvxTraceLevel.Warning, Tag, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            MvxTrace.TaggedTrace(MvxTraceLevel.Error, Tag, message, args);
        }
    }

    public class DebugTrace : IMvxTrace
    {
        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            Debug.WriteLine(tag + ":" + level + ":" + message());
        }

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
            Debug.WriteLine(tag + ":" + level + ":" + message);
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            try
            {
                Debug.WriteLine(string.Format(tag + ":" + level + ":" + message, args));
            }
            catch (FormatException)
            {
                Trace(MvxTraceLevel.Error, tag, "Exception during trace of {0} {1} {2}", level, message);
            }
        }
    }
}
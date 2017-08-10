using log4net;
using log4net.Config;

namespace GrekanMonoDaemon.Logging
{
    public static class Logger
    {
        private static readonly ILog Log;

        static Logger()
        {
            Log = LogManager.GetLogger("LOGGER");
        }

        public static void Error(object parameter)
        {
            Log.Error(parameter);
        }

        public static void Info(object message)
        {
            Log.Info(message);
        }

        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}
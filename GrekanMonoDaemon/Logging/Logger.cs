using System;
using GrekanMonoDaemon.Vk;
using log4net;
using log4net.Config;

namespace GrekanMonoDaemon.Logging
{
    public static class Logger
    {
        private static readonly ILog Log;
        private static readonly MessageSender Sender;
        private static bool UseVk;

        static Logger()
        {
            Log = LogManager.GetLogger("LOGGER");
            Sender = new MessageSender();
        }

        public static void Error(object parameter)
        {
            Log.Error(parameter);

            var exception = parameter as Exception;
            var message = exception?.Message ?? parameter.ToString();

            if (UseVk)
            {
                Sender.Send(1, "чето не рабит, а точнее " + message);
            }
        }

        public static void Info(object message)
        {
            Log.Info(message);
            if (UseVk)
            {
                Sender.Send(1, message.ToString());
            }
        }

        public static void InitLogger(bool useVk = true)
        {
            UseVk = useVk;
            XmlConfigurator.Configure();
        }
    }
}
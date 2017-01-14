using GrekanMonoDaemon.Vk;
using log4net;
using log4net.Config;

namespace GrekanMonoDaemon.Logging
{
    public static class Logger
    {
        private static readonly ILog Log;
        private static MessageSender Sender;

        static Logger()
        {
            Log = LogManager.GetLogger("LOGGER");
            Sender = new MessageSender();
        }

        public static void Error(object message)
        {
            Log.Error(message);
            Sender.Send(1, "бля чето не рабит, а точнее " + message);
        }

        public static void Info(object message)
        {
            Log.Info(message);
            Sender.Send(1, message.ToString());
        }

        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}
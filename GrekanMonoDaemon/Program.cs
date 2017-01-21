using System;
using GrekanMonoDaemon.Job;
using GrekanMonoDaemon.Logging;
using GrekanMonoDaemon.Server;
using GrekanMonoDaemon.Util;
using GrekanMonoDaemon.Vk;

namespace GrekanMonoDaemon
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Logger.InitLogger();

            var scheduler = new Scheduler();
            scheduler.Engage();

            var host = new Host();

            host.Start();
        }
    }
}
using System;
using GrekanMonoDaemon.Job;
using GrekanMonoDaemon.Logging;
using Microsoft.Owin.Hosting;

namespace GrekanMonoDaemon
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Logger.InitLogger();

//            var scheduler = new Scheduler();
//            scheduler.Engage();
            const string baseUri = "http://localhost:11002";

            WebApp.Start<Startup>(baseUri);
            Console.WriteLine("Server running at {0} - press Enter to quit. ", baseUri);
            Console.ReadLine();
        }
    }
}
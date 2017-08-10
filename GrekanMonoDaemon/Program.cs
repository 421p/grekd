using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using GrekanMonoDaemon.ImageProcessing;
using GrekanMonoDaemon.Job;
using GrekanMonoDaemon.Logging;
using GrekanMonoDaemon.Server;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

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
            
            Thread.CurrentThread.Join();
        }
    }
}
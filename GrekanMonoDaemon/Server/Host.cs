﻿using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using GrekanMonoDaemon.ImageProcessing;
using GrekanMonoDaemon.Logging;
using GrekanMonoDaemon.Util;
using NHttp;

namespace GrekanMonoDaemon.Server
{
    public class Host : HttpServer
    {
        private readonly Router _router;

        public Host()
        {
            _router = new Router();
            EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11002);
            RequestReceived += OnRequestReceived;
            UnhandledException += OnUnhandledException;
        }

        private void OnUnhandledException(object sender, HttpExceptionEventArgs args)
        {
            Logger.Error(args.Exception);
        }

        private void OnRequestReceived(object sender, HttpRequestEventArgs args)
        {
            args.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            _router.Dispatch(args);
        }
    }
}
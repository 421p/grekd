using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GrekanMonoDaemon.Server.Controllers;
using GrekanMonoDaemon.Util;
using LanguageExt;
using NHttp;

namespace GrekanMonoDaemon.Server
{
    public class Router
    {
        private readonly Dictionary<string, Action<HttpRequest, HttpResponse>> _routes;

        public Router()
        {
            _routes = LoadControllers()
                .Map(Activator.CreateInstance)
                .Cast<Controller>()
                .Map(x => new KeyValuePair<string, Action<HttpRequest, HttpResponse>>(
                    x.GetUri(), x.Handle))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public void Add(string path, Action<HttpRequest, HttpResponse> closure)
        {
            _routes[path] = closure;
        }

        public void Dispatch(HttpRequestEventArgs args)
        {
            var path = args.Request.Path;

            Console.WriteLine(path);
            
            if (_routes.ContainsKey(path))
            {
                _routes[path](args.Request, args.Response);
            }
            else
            {
                args.Response.Drop("404");
            }
        }

        private static IEnumerable<Type> LoadControllers()
        {
            return typeof(Controller).GetTypeInfo()
                .Assembly.DefinedTypes
                .Filter(type => type.IsSubclassOf(typeof(Controller)));
        }
    }
}
using NHttp;

namespace GrekanMonoDaemon.Server.Controllers
{
    public abstract class Controller
    {
        public string GetUri()
        {
            return $"/{Inflector.Inflector.Underscore(GetType().Name)}";
        }

        public abstract void Handle(HttpRequest request, HttpResponse response);
    }
}
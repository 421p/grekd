using GrekanMonoDaemon.Repository;
using GrekanMonoDaemon.Util;
using NHttp;

namespace GrekanMonoDaemon.Server.Controllers.Images
{
    public class CountImages : Controller
    {
        public override void Handle(HttpRequest request, HttpResponse response)
        {
            response.WriteLine(ImageRepository.GetCount().ToString());
        }
    }
}
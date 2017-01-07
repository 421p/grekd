using GrekanMonoDaemon.ImageProcessing;
using GrekanMonoDaemon.Repository;
using GrekanMonoDaemon.Util;
using NHttp;

namespace GrekanMonoDaemon.Server.Controllers.Posts
{
    public class CountPosts : Controller
    {
        public override void Handle(HttpRequest request, HttpResponse response)
        {
            response.WriteLine(MemesRepository.GetCount().ToString());
        }
    }
}
using System.IO;
using System.Linq;
using GrekanMonoDaemon.Repository;
using GrekanMonoDaemon.Util;
using NHttp;

namespace GrekanMonoDaemon.Server.Controllers.Images
{
    public class AddImage : Controller
    {
        public override void Handle(HttpRequest request, HttpResponse response)
        {
            if (string.IsNullOrEmpty(request.Headers["key"]))
            {
                response.Drop("no key given");
                return;
            }

            var key = KeysRepository.Find(request.Headers["key"]);

            if (key == null)
            {
                response.Drop("no key found");
                return;
            }

            if (!new [] {AccessLevel.Admin, AccessLevel.Moderator}.ToList().Contains(key.Level))
            {
                response.Drop("no access");
                return;
            }

            var ms = new MemoryStream();
            request.InputStream.CopyTo(ms);
            ImageRepository.Add(ms.ToArray());
            response.WriteLine("s u c c");
        }
    }
}
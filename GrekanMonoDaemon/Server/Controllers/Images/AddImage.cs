using System.Drawing;
using System.Drawing.Imaging;
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

            if (!new[] {AccessLevel.Admin, AccessLevel.Moderator}.ToList().Contains(key.Level))
            {
                response.Drop("no access");
                return;
            }

            if (request.Files["grekan"] == null)
            {
                response.Drop("no grekan given");
                return;
            }

            var file = request.Files["grekan"];

            using (var image = Image.FromStream(file.InputStream))
            {
                if (image.Width != 960 && image.Height != 960)
                {
                    response.Drop("image is not 960x960");
                    return;
                }

                var ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);

                ImageRepository.Add(ms.ToArray());
                response.WriteLine("s u c c");
            }
        }
    }
}
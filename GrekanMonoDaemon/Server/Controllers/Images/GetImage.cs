using System.Drawing.Imaging;
using System.Linq;
using GrekanMonoDaemon.ImageProcessing;
using GrekanMonoDaemon.Repository;
using GrekanMonoDaemon.Util;
using NHttp;

namespace GrekanMonoDaemon.Server.Controllers.Images
{
    public class GetImage : Controller
    {
        public override void Handle(HttpRequest request, HttpResponse response)
        {
            int id;

            if (!int.TryParse(request.QueryString["id"], out id))
            {
                response.Drop("Non-int id given", 403);
                return;
            }

            if (id < 0 || id >= ImageRepository.GetCount())
            {
                response.Drop("id not in range");
                return;
            }

            response.ContentType = "image/jpeg";

            var task = ImageRepository.GetImageRaw(id);

            task.Wait();

            var bytes = task.Result;

            response.OutputStream.Write(bytes, 0, bytes.Length);
        }
    }
}
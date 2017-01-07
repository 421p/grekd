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

            var t = ImageRepository.GetImage(id);

            try
            {
                t.Wait();
                t.Result.Save(response.OutputStream, ImageFormat.Jpeg);
            }
            finally
            {
                t.Result.Dispose();
            }
        }
    }
}
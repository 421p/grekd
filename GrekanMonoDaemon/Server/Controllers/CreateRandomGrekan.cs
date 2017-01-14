using System.Drawing.Imaging;
using GrekanMonoDaemon.ImageProcessing;
using GrekanMonoDaemon.Util;
using NHttp;

namespace GrekanMonoDaemon.Server.Controllers
{
    public class CreateRandomGrekan : Controller
    {
        public override void Handle(HttpRequest request, HttpResponse response)
        {
            response.ContentType = "image/jpeg";
            var task = ImageFactory.Generate();

            int w;
            int.TryParse(request.QueryString["w"], out w);

            using (var image = task.Result)
            {
                image.Save(response.OutputStream, ImageFormat.Jpeg);
            }
        }
    }
}
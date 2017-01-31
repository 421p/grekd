using System.Drawing.Imaging;
using GrekanMonoDaemon.ImageProcessing;
using NHttp;

namespace GrekanMonoDaemon.Server.Controllers
{
    public class CreateRandomGrekan : Controller
    {
        public override void Handle(HttpRequest request, HttpResponse response)
        {
            response.ContentType = "image/jpeg";
            var task = ImageFactory.Generate(true);

            using (var image = task.Result)
            {
                image.Save(response.OutputStream, ImageFormat.Jpeg);
            }
        }
    }
}
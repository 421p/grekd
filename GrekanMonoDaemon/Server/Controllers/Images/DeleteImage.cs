using GrekanMonoDaemon.Repository;
using GrekanMonoDaemon.Util;
using NHttp;

namespace GrekanMonoDaemon.Server.Controllers.Images
{
    public class DeleteImage : Controller
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

            if (key.Level != AccessLevel.Admin)
            {
                response.Drop("no access");
                return;
            }

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

            ImageRepository.Delete(id);
            
            ImageRepository.ResetInternalPointer();
            
            response.WriteLine("s u c c");
        }
    }
}
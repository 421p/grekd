using GrekanMonoDaemon.Repository;
using GrekanMonoDaemon.Util;
using NHttp;

namespace GrekanMonoDaemon.Server.Controllers.Posts
{
    public class GetPost : Controller
    {
        public override void Handle(HttpRequest request, HttpResponse response)
        {
            int id;

            if (!int.TryParse(request.QueryString["id"], out id))
            {
                response.Drop("Non-int id given", 403);
                return;
            }

            if (id < 0 || id >= MemesRepository.GetCount())
            {
                response.Drop("id not in range");
                return;
            }

            var t = MemesRepository.GetById(id);
            t.Wait();

            response.WriteJson(t.Result);
        }
    }
}
using System;
using GrekanMonoDaemon.Repository;
using GrekanMonoDaemon.Util;
using NHttp;

namespace GrekanMonoDaemon.Server.Controllers.Posts
{
    public class GetPostInRange : Controller
    {
        public override void Handle(HttpRequest request, HttpResponse response)
        {
            DateTime from;

            if (!DateTime.TryParse(request.QueryString["from"], out from))
            {
                response.Drop("Incorrect date given", 403);
                return;
            }

            DateTime to;

            if (!DateTime.TryParse(request.QueryString["to"], out to))
            {
                response.Drop("Incorrect date given", 403);
                return;
            }

            var t = MemesRepository.GetByDate(from, to);
            t.Wait();

            response.WriteJson(t.Result);
        }
    }
}
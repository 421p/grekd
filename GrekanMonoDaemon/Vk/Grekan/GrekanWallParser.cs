using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace GrekanMonoDaemon.Vk.Grekan
{
    public class GrekanWallParser : VkParser
    {
        public List<Post> Get(ulong count = 50)
        {
            var posts = new List<Post>();

            OnParse += () =>
            {
                posts.AddRange(Api.Wall.Get(new WallGetParams
                    {
                        Count = count,
                        OwnerId = (long)Config.grekan_id,
                        Filter = WallFilter.Owner
                    })
                    .WallPosts);
            };

            Parse();

            return posts;
        }

        public List<Post> GetAll()
        {
            ulong totalCount = ulong.MaxValue;

            ulong offset = 0;

            var posts = new List<Post>();

            OnParse += () =>
            {
                try
                {
                    var part = Api.Wall.Get(new WallGetParams
                    {
                        Count = 100,
                        OwnerId = Config.grekan_id,
                        Filter = WallFilter.Owner,
                        Offset = offset
                    });

                    totalCount = part.TotalCount;

                    posts.AddRange(part.WallPosts);
                }
                catch (InvalidParameterException)
                {
                }
            };

            for (; offset < totalCount; offset += 100)
            {
                Console.WriteLine($"Offset: {offset}");
                Parse();
                Thread.Sleep(500);
            }

            return posts;
        }
    }
}
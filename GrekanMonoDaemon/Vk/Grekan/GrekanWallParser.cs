using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GrekanMonoDaemon.Logging;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace GrekanMonoDaemon.Vk.Grekan
{
    public class GrekanWallParser : VkCommandExecutor
    {
        public List<Post> Get(ulong count = 50)
        {
            var posts = new List<Post>();

            OnExecute += () =>
            {
                posts.AddRange(Api.Wall.Get(new WallGetParams
                    {
                        Count = count,
                        OwnerId = (long) Config.grekan_id,
                        Filter = WallFilter.Owner
                    })
                    .WallPosts);
            };

            Execute();

            return posts;
        }

        public List<Post> GetAll()
        {
            ulong totalCount = ulong.MaxValue;

            ulong offset = 0;

            var posts = new List<Post>();

            OnExecute += () =>
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
                catch (Exception e)
                {
                    Logger.Error($"Failed to get wall: {e.Message}");
                }
            };

            for (; offset < totalCount; offset += 100)
            {
                Console.WriteLine($"Offset: {offset}");
                Execute();
                Thread.Sleep(500);
            }

            return posts;
        }
    }
}
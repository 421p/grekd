using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GrekanMonoDaemon.ImageProcessing;
using GrekanMonoDaemon.Logging;
using GrekanMonoDaemon.Repository;
using LanguageExt;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;

namespace GrekanMonoDaemon.Vk
{
    public class GedPublisher : VkCommandExecutor
    {
        private readonly HttpClient _client;

        public GedPublisher()
        {
            _client = new HttpClient();

            OnExecute += async () =>
            {
                await PostImage();
                Logger.Info("Posted image on grekaneveryday.");
                Logger.Info("Checking for posts with no likes...");
                CheckWall();
                Logger.Info("Done...");
            };
        }

        private async Task PostImage()
        {
            var server = Api.Photo.GetWallUploadServer((long) Config.pabloses.grekaneveryday);

            var image = await ImageFactory.Generate();

            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;

            var response = await _client.PostAsync(
                server.UploadUrl,
                new MultipartFormDataContent {{new StreamContent(ms), "photo", "grek.jpg"}}
            );

            var respString = await response.Content.ReadAsStringAsync();

            Api.Wall.Post(new WallPostParams
            {
                OwnerId = -(long) Config.pabloses.grekaneveryday,
                FromGroup = true,
                Attachments = Api.Photo.SaveWallPhoto(
                    respString, (ulong) Config.bot_id, (ulong) Config.pabloses.grekaneveryday
                )
            });
        }

        private void CheckWall()
        {
            var posts = Api.Wall.Get(new WallGetParams
                {
                    Count = 10,
                    OwnerId = -(long) Config.pabloses.grekaneveryday,
                    Filter = WallFilter.Owner
                })
                .WallPosts;

            var ids = posts.Filter(post => post.Likes.Count == 0 && post.Date.HasValue)
                .Filter(post => (DateTime.Now - post.Date.Value).TotalHours > 3)
                .Map(post => post.Id);

            foreach (var id in ids)
            {
                Logger.Info($"Deleting post with id: {id}");
                Api.Wall.Delete(-(long) Config.pabloses.grekaneveryday, id);
            }
        }
    }
}
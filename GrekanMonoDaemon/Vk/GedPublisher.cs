using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using GrekanMonoDaemon.ImageProcessing;
using GrekanMonoDaemon.Logging;
using GrekanMonoDaemon.Repository;
using VkNet.Model.RequestParams;

namespace GrekanMonoDaemon.Vk
{
    public class GedPublisher : VkPublisher
    {
        private HttpClient _client;

        public GedPublisher()
        {
            _client = new HttpClient();

            OnPublish += async () =>
            {
                var server = Api.Photo.GetWallUploadServer((long)Config.pabloses.grekaneveryday);

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
                    OwnerId = -(long)Config.pabloses.grekaneveryday,
                    FromGroup = true,
                    Attachments = Api.Photo.SaveWallPhoto(
                        respString, (ulong)Config.bot_id, (ulong) Config.pabloses.grekaneveryday
                    )
                });

                Logger.Log.Info("Posted image on grekaneveryday.");
            };
        }
    }
}
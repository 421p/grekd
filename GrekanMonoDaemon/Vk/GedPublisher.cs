using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using GrekanMonoDaemon.ImageProcessing;
using GrekanMonoDaemon.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using VkNet.Model.RequestParams;

namespace GrekanMonoDaemon.Vk
{
    public class GedPublisher : VkCommandExecutor
    {
        private readonly HttpClient _client;

        private readonly TelegramBotClient _telegram;

        private readonly TwitterCredentials _twicreds;

        public GedPublisher()
        {
            _twicreds = new TwitterCredentials(
                "HwAhIVjZhs0OMlGM16QmcLrBu",
                "o1ZqEUam6a2AeXQQq6m5TcQiyY8OlAYXTcdRnZSHAeiiTElvfl",
                "894005716381552641-nimtb10kb4THV4M514yyTrLKTimrEHJ",
                "eioGLyFWpAx8rxo0pz6KMlVB9uRCqTpPqJRCyDUhCe6KV"
            );

            _client = new HttpClient();
            _telegram = new TelegramBotClient((string) Config.telegram.grekaneveryday.token);

            OnExecute += async () =>
            {
                await PostImage();
                Logger.Info("Posted image on grekaneveryday.");
            };
        }

        private async Task PostImage()
        {
            var server = Api.Photo.GetWallUploadServer((long) Config.pabloses.grekaneveryday);

            var data = await ImageFactory.Generate();
            var post = data.Item2;

            var ms = new MemoryStream();
            data.Item1.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;

            var response = await _client.PostAsync(
                server.UploadUrl,
                new MultipartFormDataContent {{new StreamContent(ms), "photo", "grek.jpg"}}
            );

            ms.Position = 0;

//            Tweetinvi.Auth.ExecuteOperationWithCredentials(_twicreds, () =>
//                Tweet.PublishTweet(post.Text, new PublishTweetOptionalParameters
//                {
//                    Medias =
//                    {
//                        Tweetinvi.Auth.ExecuteOperationWithCredentials(
//                            _twicreds,
//                            () => Upload.UploadImage(ms.ToArray())
//                        )
//                    }
//                }));

            await _telegram.SendPhotoAsync(
                (string) Config.telegram.grekaneveryday.channel,
                new FileToSend("grek.jpg", ms)
            );

            var text = $"[{post.Date}] {post.Text}";

            await _telegram.SendTextMessageAsync("@textgrekaneveryday", text);

            var respString = await response.Content.ReadAsStringAsync();

            Api.Wall.Post(new WallPostParams
            {
                OwnerId = -(long) Config.pabloses.grekaneveryday,
                FromGroup = true,
                Attachments = Api.Photo.SaveWallPhoto(
                    respString, (ulong) Config.bot_id, (ulong) Config.pabloses.grekaneveryday
                ),
                Message = post.Text
            });
        }
    }
}
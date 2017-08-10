using GrekanMonoDaemon.Repository;
using MongoDB.Driver;
using Quartz;
using Telegram.Bot;

namespace GrekanMonoDaemon.Job
{
    public class GrekileaksPublish : IJob
    {
        public async void Execute(IJobExecutionContext context)
        {
            var config = new MongoClient().GetDatabase("grekileaks")
                .GetCollection<dynamic>("configs")
                .Find(FilterDefinition<dynamic>.Empty)
                .First();
            
            var telega = new TelegramBotClient((string) config.telegram.grekaneveryday.token);

            var post = await MemesRepository.GetRandom();

            var text = $"[{post.Date}] {post.Text}";
            
            await telega.SendTextMessageAsync("@grekileaks", text);
        }
    }
}
using MongoDB.Driver;
using VkNet;
using VkNet.Enums.Filters;

namespace GrekanMonoDaemon.Vk
{
    public abstract class BaseVk
    {
        protected dynamic Config;
        protected VkApi Api => SharedApi.Api;

        protected BaseVk()
        {
            Config = new MongoClient().GetDatabase("grekileaks")
                .GetCollection<dynamic>("configs")
                .Find(FilterDefinition<dynamic>.Empty)
                .First();
        }

        protected void Auth()
        {
            Api.Authorize(new ApiAuthParams
            {
                ApplicationId = (ulong)Config.user.app_id,
                Login = Config.user.login,
                Password = Config.user.password,
                Settings = Settings.All
            });
        }
    }
}
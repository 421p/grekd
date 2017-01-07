using System;
using GrekanMonoDaemon.Logging;
using VkNet.Exception;

namespace GrekanMonoDaemon.Vk
{
    public abstract class VkPublisher : BaseVk
    {
        public void Publish()
        {
            try
            {
                if (!Api.IsAuthorized)
                {
                    Auth();
                }

                OnPublish?.Invoke();
            }
            catch (Exception e)
            {
                if (e is AccessTokenInvalidException)
                {
                    Logger.Log.Error("Trying to renew token...");
                    Auth();
                    OnPublish?.Invoke();
                }
                else
                {
                    Logger.Log.Error(e);
                }
            }
        }

        protected event Action OnPublish;
    }
}
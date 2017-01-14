using System;
using GrekanMonoDaemon.Logging;
using VkNet.Exception;

namespace GrekanMonoDaemon.Vk
{
    public abstract class VkCommandExecutor : BaseVk
    {
        public void Execute()
        {
            try
            {
                if (!Api.IsAuthorized)
                {
                    Auth();
                }

                OnExecute?.Invoke();
            }
            catch (Exception e)
            {
                if (e is AccessTokenInvalidException)
                {
                    Logger.Error("Trying to renew token...");
                    Auth();
                    OnExecute?.Invoke();
                }
                else
                {
                    Logger.Error(e);
                }
            }
        }

        protected event Action OnExecute;
    }
}
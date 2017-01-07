using System;
using VkNet.Exception;

namespace GrekanMonoDaemon.Vk
{
    public abstract class VkParser : BaseVk
    {
        public void Parse()
        {
            try
            {
                if (!Api.IsAuthorized)
                {
                    Auth();
                }

                OnParse?.Invoke();
            }
            catch (AccessTokenInvalidException)
            {
                Auth();
                OnParse?.Invoke();
            }
        }

        protected event Action OnParse;
    }
}
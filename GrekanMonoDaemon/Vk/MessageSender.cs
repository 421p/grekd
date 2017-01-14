using VkNet.Model.RequestParams;

namespace GrekanMonoDaemon.Vk
{
    public class MessageSender : VkCommandExecutor
    {
        private long _receiver;
        private string _message;

        public MessageSender()
        {
            OnExecute += () =>
            {
                Api.Messages.Send(new MessagesSendParams
                {
                    Message = _message,
                    ChatId = _receiver
                });
            };
        }

        public void Send(long receiver, string message)
        {
            _receiver = receiver;
            _message = message;
            Execute();
        }
    }
}
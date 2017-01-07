using VkNet;

namespace GrekanMonoDaemon.Vk
{
    public static class SharedApi
    {
        private static VkApi _vk;
        public static VkApi Api => _vk ?? (_vk = new VkApi());
    }
}
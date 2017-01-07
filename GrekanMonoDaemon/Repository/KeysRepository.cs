using GrekanMonoDaemon.Util;
using MongoDB.Driver;

namespace GrekanMonoDaemon.Repository
{
    public static class KeysRepository
    {
        private static readonly MongoClient _client;
        private static readonly IMongoCollection<Key> _keys;

        static KeysRepository()
        {
            _client = new MongoClient();
            _keys = _client.GetDatabase("grekileaks").GetCollection<Key>("keys");
        }

        public static Key Find(string key)
        {
            return _keys.Find(x => x.Value == key).FirstOrDefault();
        }
    }
}
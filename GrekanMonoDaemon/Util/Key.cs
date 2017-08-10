using MongoDB.Bson;

namespace GrekanMonoDaemon.Util
{
    public class Key
    {
        public ObjectId Id { get; set; }
        public string Value { get; set; }
        public string Level { get; set; }
        public string Note { get; set; }
    }
}
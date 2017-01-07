using System;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace GrekanMonoDaemon.Vk.Simplification
{
    public class SimplePost
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public static implicit operator string(SimplePost post) => post.ToString();

        public override string ToString() => Text;
    }
}
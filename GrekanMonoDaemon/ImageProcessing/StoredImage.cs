using MongoDB.Bson;

namespace GrekanMonoDaemon.ImageProcessing
{
    public class StoredImage
    {
        public ObjectId Id { get; set; }
        public byte[] Bytes { get; set; }
    }
}
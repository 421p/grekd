﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GrekanMonoDaemon.ImageProcessing;
using GrekanMonoDaemon.Util;
using MongoDB.Driver;

namespace GrekanMonoDaemon.Repository
{
    public static class ImageRepository
    {
        private static readonly MongoClient _client;
        private static readonly IMongoCollection<StoredImage> _images;
        private static readonly GRandom _generator;

        public static async Task<long> GetCountAsync()
        {
            return await _images.CountAsync(FilterDefinition<StoredImage>.Empty);
        }

        public static long GetCount()
        {
            return _images.Count(FilterDefinition<StoredImage>.Empty);
        }

        static ImageRepository()
        {
            _client = new MongoClient();
            var db = _client.GetDatabase("grekileaks");
            _images = db.GetCollection<StoredImage>("images");
            _generator = new GRandom();
        }

        public static void Add(byte[] bytes)
        {
            _images.InsertOne(new StoredImage { Bytes = bytes });
        }

        public static void Delete(int id)
        {
            var image = _images.Find(FilterDefinition<StoredImage>.Empty)
                .ToEnumerable()
                .ElementAt(id);

            _images.FindOneAndDelete(x => x.Id == image.Id);
        }

        public static async Task<Image> GetImage()
        {
            var ms = new MemoryStream(await GetImageRaw());

            return Image.FromStream(ms);
        }

        public static async Task<byte[]> GetImageRaw()
        {
            var data = await _images.Find(FilterDefinition<StoredImage>.Empty)
                .Limit(-1)
                .Skip(_generator.Next(0, await GetCountAsync() - 1))
                .FirstAsync();

            return data.Bytes;
        }

        public static async Task<Image> GetImage(int id)
        {
            var ms = new MemoryStream(await GetImageRaw(id));

            return Image.FromStream(ms);
        }

        public static async Task<byte[]> GetImageRaw(int id)
        {
            var data = await _images.FindAsync(FilterDefinition<StoredImage>.Empty);

            return data.ToEnumerable().ElementAt(id).Bytes;
        }
    }
}
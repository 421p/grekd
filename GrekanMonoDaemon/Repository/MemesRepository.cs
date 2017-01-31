using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GrekanMonoDaemon.Logging;
using GrekanMonoDaemon.Util;
using GrekanMonoDaemon.Vk.Grekan;
using GrekanMonoDaemon.Vk.Simplification;
using LanguageExt;
using MongoDB.Driver;

namespace GrekanMonoDaemon.Repository
{
    public static class MemesRepository
    {
        private static readonly MongoClient _client;
        private static readonly IMongoCollection<SimplePost> _posts;
        private static readonly GrekanWallParser _parser;
        private static readonly Random _rand;

        static MemesRepository()
        {
            _rand = new Random((int) DateTime.Now.Ticks);
            _parser = new GrekanWallParser();
            _client = new MongoClient();
            _posts = _client.GetDatabase("grekileaks").GetCollection<SimplePost>("posts");
        }

        public static long GetCount() => _posts.Count(FilterDefinition<SimplePost>.Empty);

        public static async Task<SimplePost> GetById(int id)
        {
            var data = await _posts.FindAsync(FilterDefinition<SimplePost>.Empty);
            return data.ToEnumerable().ElementAtOrDefault(id);
        }

        public static async Task<IEnumerable<SimplePost>> GetByDate(DateTime from, DateTime to)
        {
            var data = await _posts.FindAsync(x => x.Date >= from && x.Date <= to);
            return data.ToEnumerable();
        }

        public static async Task<SimplePost> GetRandom(int max = 120)
        {
            var size = await _posts.CountAsync(FilterDefinition<SimplePost>.Empty);

            while (true)
            {
                var post = await _posts.Find(FilterDefinition<SimplePost>.Empty)
                    .Limit(-1)
                    .Skip(_rand.Next(0, (int) size - 1))
                    .FirstAsync();

                if (post.Text.Length != 0 && post.Text.Length <= 120 && !Regex.IsMatch(post.Text, @"[\/\[\]\|]"))
                {
                    return post;
                }
            }
        }

        public static async Task Sync()
        {
            Logger.Info("Syncing posts with Grekan's wall...");

            var latest = _parser.Get().Map(x => x.Simplify());

            foreach (var post in latest)
            {
                var count = await _posts.CountAsync(x => x.Date == post.Date);

                if (count == 0)
                {
                    Console.WriteLine($"Inserting post from: {post.Date}");
                    await _posts.InsertOneAsync(post);
                }
            }

            Logger.Info("Done.");
        }
    }
}
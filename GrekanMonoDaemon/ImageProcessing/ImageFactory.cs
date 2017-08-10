using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GrekanMonoDaemon.Logging;
using GrekanMonoDaemon.Repository;
using GrekanMonoDaemon.Util;
using GrekanMonoDaemon.Vk.Simplification;
using VkNet.Model;

namespace GrekanMonoDaemon.ImageProcessing
{
    public static class ImageFactory
    {
        private static readonly Font Font;
        private static readonly StringFormat LineFormat;
        private static int _imageWidth;
        private static int _imageHeight;

        static ImageFactory()
        {
            Font = new Font("DejaVuSansMono", 32, FontStyle.Regular);

            LineFormat = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
        }

        public static async Task<Tuple<Image, SimplePost>> Generate(bool suppressLog = false)
        {
            var img = await ImageRepository.GetImage();
            _imageWidth = img.Width;
            _imageHeight = img.Height;

            var bitok = new Bitmap(_imageWidth, _imageHeight);

            using (var context = Graphics.FromImage(bitok))
            {
                context.DrawImage(img, 0, 0);

                var post = await WriteText(context, suppressLog);
                return new Tuple<Image, SimplePost>(bitok, post);
            }
        }

        private static async Task<SimplePost> WriteText(Graphics context, bool suppressLog)
        {
            const int verticalMargin = 100;
            const int lineWidth = 30;
            const int lineSpacing = 15;

            var post = await MemesRepository.GetRandom();

            var quote = HttpUtility.HtmlDecode(post.Text)?.Replace("<br>", "\n").Replace("\n", " ");

            if (!suppressLog)
            {
                Logger.Info($"Generated grekan with text: {post.Text}\n Original text was created in: {post.Date}");
            }

            var chunks = quote.SplitByLength(lineWidth).Reverse().ToArray();

            for (var i = 0; i < chunks.Length; i++)
            {
                context.DrawOutlinedString(chunks[i], Font, LineFormat, Brushes.Black, Brushes.White,
                    new PointF(_imageWidth / 2, _imageHeight - verticalMargin - i * (Font.Size + lineSpacing)));
            }

            context.Save();

            return post;
        }
    }
}
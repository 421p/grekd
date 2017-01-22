using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using GrekanMonoDaemon.Vk.Simplification;
using Newtonsoft.Json;
using NHttp;
using VkNet.Model;

namespace GrekanMonoDaemon.Util
{
    public static class Utils
    {
        public static Bitmap Resize(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void Drop(this HttpResponse resp, string message, int code = 200)
        {
            resp.StatusCode = code;

            using (var writer = new StreamWriter(resp.OutputStream))
            {
                writer.Write(message);
            }
        }

        public static void WriteImage(this HttpResponse resp, byte[] message)
        {
            resp.ContentType = "image/jpeg";

            using (var writer = new BinaryWriter(resp.OutputStream))
            {
                writer.Write(message);
            }
        }

        public static void WriteJson(this HttpResponse resp, object message)
        {
            resp.ContentType = "application/json";

            using (var writer = new StreamWriter(resp.OutputStream))
            {
                writer.Write(JsonConvert.SerializeObject(message));
            }
        }

        public static void WriteLine(this HttpResponse resp, string message)
        {
            using (var writer = new StreamWriter(resp.OutputStream))
            {
                writer.Write(message);
            }
        }

        public static SimplePost Simplify(this Post post)
        {
            return new SimplePost
            {
                Date = post.Date.GetValueOrDefault(),
                Text = post.Text
            };
        }

        public static IEnumerable<string> SplitByLength(this string str, int maxLength)
        {
            var words = str.Split(' ');
            var line = new StringBuilder();

            foreach (var word in words)
            {
                if (line.Length + word.Length >= maxLength)
                {
                    yield return line.ToString();
                    line.Clear();
                }

                line.AppendFormat("{0}{1}", line.Length > 0 ? " " : "", word);
            }

            yield return line.ToString();
        }

        public static void DrawOutlinedString(
            this Graphics context,
            string text,
            Font font,
            StringFormat format,
            Brush outlineBrush,
            Brush textBrush,
            PointF point
        )
        {
            var outliner = new Action<int, int>((x, y) =>
                context.DrawString(text,
                    font,
                    outlineBrush,
                    point.X + x,
                    point.Y + y,
                    format
                ));

            outliner(+2, +2);
            outliner(+0, +3);
            outliner(-2, +2);
            outliner(-3, +0);
            outliner(-2, -2);
            outliner(+0, -3);
            outliner(+2, -2);
            outliner(+3, +0);

            context.DrawString(text, font, textBrush, point, format);
        }
    }
}
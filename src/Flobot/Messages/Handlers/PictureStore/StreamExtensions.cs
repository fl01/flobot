using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Flobot.Messages.Handlers.PictureStore
{
    public static class StreamExtensions
    {
        private static Dictionary<string, string> ImageTypes;
        static StreamExtensions()
        {
            ImageTypes = new Dictionary<string, string>();
            ImageTypes.Add("FFD8", "jpg");
            ImageTypes.Add("424D", "bmp");
            ImageTypes.Add("89504E470D0A1A0A", "png");
        }

        public static bool IsImage(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            StringBuilder builder = new StringBuilder();
            int largestByteHeader = ImageTypes.Max(img => img.Key.Length);

            for (int i = 0; i < largestByteHeader; i++)
            {
                string bit = stream.ReadByte().ToString("X2");
                builder.Append(bit);

                string builtHex = builder.ToString();
                bool isImage = ImageTypes.Keys.Any(img => img == builtHex);
                if (isImage)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
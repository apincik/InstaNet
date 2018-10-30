using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace Bot.Utils
{
    /// <summary>
    /// Util for remote image download.
    /// </summary>
    public static class Image
    {
        public static string DownloadImage(string url)
        {
            //Download image as TempFile
            var tempFilePath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".jpg";
            using (WebClient client = new WebClient())
            {
                Stream stream = client.OpenRead(url);
                Bitmap bitmap; bitmap = new Bitmap(stream);

                if (bitmap != null)
                    bitmap.Save(tempFilePath, ImageFormat.Jpeg);

                stream.Flush();
                stream.Close();
            }

            return tempFilePath;
        }

    }
}

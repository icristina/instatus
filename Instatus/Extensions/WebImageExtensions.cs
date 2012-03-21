using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Instatus
{
    public static class WebImageExtensions
    {
        public static WebImage Square(this WebImage image, int size)
        {
            int offset;

            if (image.Height > image.Width) //portrait
            {
                offset = (image.Height - image.Width) / 2;
                image.Crop(offset, 0, offset, 0);
            }
            else // landscape
            {
                offset = (image.Width - image.Height) / 2;
                image.Crop(0, offset, 0, offset);
            }

            image.Resize(size, size);
            return image;
        }

        public static WebImage BoundingBox(this WebImage image, int width, int height = 0)
        {
            image.Resize(width, height > 0 ? height : width, true, true);
            return image;
        }

        public static Stream GetJpgAsStream(this WebImage image)
        {
            return image.GetBytes("jpeg").ToStream();
        }

        public static Graphics AsHighQuality(this Graphics graphics)
        {
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;            
            return graphics;
        }

        // http://stackoverflow.com/questions/249587/high-quality-image-scaling-c-sharp
        public static Bitmap Resize(Image image, int width, int height)
        {
            var bitmap = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(bitmap).AsHighQuality())
            {
                graphics.DrawImage(image, 0, 0, bitmap.Width, bitmap.Height);
            }

            return bitmap;
        }
    }
}
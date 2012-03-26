using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
            return image.Resize(width, height > 0 ? height : width, true, true);
        }

        // http://stackoverflow.com/questions/249587/high-quality-image-scaling-c-sharp    
        public static Graphics AsHighQuality(this Graphics graphics)
        {
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;            
            return graphics;
        }

        // WebImage puts a grey border on the top and left edge of image
        // http://forums.asp.net/t/1633723.aspx/1
        public static Image Resize(this Image image, int width, int height, bool preserveAspectRatio = false, bool preventEnlarge = false)
        {
            var originalDimensions = new Tuple<int, int>(image.Width, image.Height);
            var newDimensions = new Tuple<int, int>(width, height);

            newDimensions = originalDimensions.Resize(newDimensions, preserveAspectRatio, preventEnlarge);

            width = newDimensions.Item1;
            height = newDimensions.Item2;
            
            var isIndexedColor = (((image.PixelFormat == PixelFormat.Format1bppIndexed) || (image.PixelFormat == PixelFormat.Format4bppIndexed)) || (image.PixelFormat == PixelFormat.Format8bppIndexed)) || (image.PixelFormat == PixelFormat.Indexed);
            var bitmap = isIndexedColor ? new Bitmap(width, height) : new Bitmap(width, height, image.PixelFormat);

            if (preserveAspectRatio)
            {
                bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            }
            else
            {
                bitmap.SetResolution(96f, 96f);
            }

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(Brushes.White, 0, 0, width, height);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic; // cannot be prior to rectangle or creates incorrect border
                graphics.DrawImage(image, 0, 0, width, height);
            }
            
            return bitmap;
        }

        public static Tuple<int, int> Resize(this Tuple<int, int> originalDimensions, Tuple<int, int> newDimensions, bool preserveAspectRatio = false, bool preventEnlarge = false)
        {
            int originalWidth = originalDimensions.Item1;
            int originalHeight = originalDimensions.Item2;

            int width = newDimensions.Item1;
            int height = newDimensions.Item2;
            
            if (preserveAspectRatio)
            {
                double num3 = (height * 100.0) / ((double)originalHeight);
                double num4 = (width * 100.0) / ((double)originalWidth);
                if (num3 > num4)
                {
                    height = (int)Math.Round((double)((num4 * originalHeight) / 100.0));
                }
                else if (num3 < num4)
                {
                    width = (int)Math.Round((double)((num3 * originalWidth) / 100.0));
                }
            }

            if (preventEnlarge)
            {
                if (height > originalHeight)
                {
                    height = originalHeight;
                }
                if (width > originalWidth)
                {
                    width = originalWidth;
                }
            }

            return new Tuple<int, int>(width, height);
        }

        public static Image BoundingBox(this Image image, int width, int height = 0)
        {
            return image.Resize(width, height > 0 ? height : width, true, true);
        }
    }
}
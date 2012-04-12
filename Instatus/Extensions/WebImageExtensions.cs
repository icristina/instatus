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
        // http://stackoverflow.com/questions/249587/high-quality-image-scaling-c-sharp    
        public static Graphics AsHighQuality(this Graphics graphics)
        {
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;            
            return graphics;
        }

        // http://aspnetwebstack.codeplex.com/SourceControl/changeset/view/30bf6af9da1b#src%2fSystem.Web.Helpers%2fWebImage.cs
        private const float FixedResolution = 96f;

        private static Bitmap GetBitmapFromImage(Image image, int width, int height, bool preserveResolution = true)
        {
            bool indexed = (image.PixelFormat == PixelFormat.Format1bppIndexed ||
                            image.PixelFormat == PixelFormat.Format4bppIndexed ||
                            image.PixelFormat == PixelFormat.Format8bppIndexed ||
                            image.PixelFormat == PixelFormat.Indexed);

            Bitmap bitmap = indexed ? new Bitmap(width, height) : new Bitmap(width, height, image.PixelFormat);
            
            if (preserveResolution)
            {
                bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            }
            else
            {
                bitmap.SetResolution(FixedResolution, FixedResolution);
            }

            using (Graphics graphic = Graphics.FromImage(bitmap).AsHighQuality())
            {
                if (indexed)
                {
                    graphic.FillRectangle(Brushes.White, 0, 0, width, height);
                }

                // offset stops 1px border on top and left
                int widthOffset = 1;
                var aspectRatio = (double)width / (double)height;
                var heightOffset = (int)Math.Round((double)widthOffset / aspectRatio);

                graphic.DrawImage(image, - widthOffset, - widthOffset, width + widthOffset, height + heightOffset);
            }

            return bitmap;
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
            
            return GetBitmapFromImage(image, width, height, preserveAspectRatio);
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

        public static Image BoundingBox(this Image image, int width)
        {
            return image.Resize(width, width, true, true);
        }

        public static Image BoundingBox(this Image image, int width, int height)
        {
            return image.Resize(width, height, true, true);
        }

        public static Image Crop(this Image image, int top, int right, int bottom, int left)
        {
            if ((top + bottom > image.Height) || (left + right > image.Width))
            {
                return image;
            }

            int width = image.Width - (left + right);
            int height = image.Height - (top + bottom);

            RectangleF rect = new RectangleF(left, top, width, height);

            using (Bitmap bitmap = GetBitmapFromImage(image, image.Width, image.Height))
            {
                try
                {
                    return bitmap.Clone(rect, image.PixelFormat);
                }
                catch (OutOfMemoryException)
                {
                    return image;
                }
            }
        }

        public static Image Square(this Image image, int size)
        {
            using (var croppedImage = image.AspectRatio(size, size))
            {
                return croppedImage.Resize(size, size);
            }
        }

        public static Image Mask(this Image image, int width, int height)
        {
            using (var croppedImage = image.AspectRatio(width, height))
            {
                return croppedImage.Resize(width, height);
            }
        }

        public static Image AspectRatio(this Image image, int width, int height)
        {
            int offset;

            double imageAspectRatio = (double)image.Width / (double)image.Height;
            double cropAspectRatio = (double)width / (double)height;

            if (imageAspectRatio == cropAspectRatio)
            {
                return image;
            }
            if (cropAspectRatio > imageAspectRatio)
            {
                offset = (image.Height - (height * (image.Width / width))) / 2;
                return image.Crop(offset, 0, offset, 0);
            }
            else
            {
                offset = (image.Width - (width * (image.Height / height))) / 2;
                return image.Crop(0, offset, 0, offset);
            }
        }
    }
}
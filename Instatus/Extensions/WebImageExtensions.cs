using System;
using System.Collections.Generic;
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
    }
}
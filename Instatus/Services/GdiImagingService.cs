using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Services
{
    public class GdiImagingService : IImagingService
    {
        public Stream Crop(Stream stream, Element area)
        {
            using (var originalImage = (Bitmap)Bitmap.FromStream(stream))
            using (var resizedImage = originalImage.Crop(area))
            using (var outputStream = new MemoryStream())
            {
                resizedImage.Save(outputStream, ImageFormat.Jpeg);
                return outputStream;
            }
        }

        public Stream Transform(Stream stream, Transform transform)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Instatus.Services;
using System.Web.Helpers;
using Instatus.Web;
using System.Drawing;
using System.Drawing.Imaging;
using Instatus.Data;

namespace Instatus.Services
{
    public interface IBlobService
    {
        string Save(string contentType, string slug, Stream stream);
        Stream Stream(string key);
        string[] Query();
    }
}

namespace Instatus
{
    public static class BlobServiceExtensions
    {
        public static Image LoadImage(this IBlobService blobService, string key)
        {
            using (var stream = blobService.Stream(key))
            {
                return Bitmap.FromStream(stream);
            }
        }

        public static string SaveImage(this IBlobService blobService, Stream stream)
        {
            try
            {
                var image = Bitmap.FromStream(stream);
                return blobService.SaveImage(image, Generator.TimeStamp());
            }
            catch
            {
                return null;
            }
        }

        public static string SaveImage(this IBlobService blobService, Image image, string key)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Jpeg);
                return blobService.Save("image/jpeg", key, memoryStream);
            }
        }
        
        public static void GenerateThumbnail(this IBlobService blobService, string key, int boundingBoxSize = 200, bool alwaysCreate = false) 
        {
            if (!alwaysCreate && blobService.HasThumbnail(key))
                return;
            
            var thumbnailKey = WebPath.Resize(WebSize.Thumb, key);

            using (var stream = blobService.Stream(key)) 
            using (var originalImage = (Bitmap)Bitmap.FromStream(stream))
            using (var resizedImage = originalImage.BoundingBox(boundingBoxSize))
            {
                blobService.SaveImage(resizedImage, thumbnailKey);
            }
        }

        public static bool Exists(this IBlobService blobService, string key) 
        {
            using (var stream = blobService.Stream(key)) 
            {
                return stream != null;
            }
        }

        public static bool HasThumbnail(this IBlobService blobService, string key) 
        {
            var thumbnailKey = WebPath.Resize(WebSize.Thumb, key);
            return blobService.Exists(thumbnailKey);
        }
    }
}
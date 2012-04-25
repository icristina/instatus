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
using Instatus.Models;

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
                var key = blobService.SaveImage(image, Generator.TimeStamp());

                foreach (var size in WebCatalog.ImageSizes)
                {
                    blobService.GenerateSize(key, size.Key, size.Value, false);
                }

                return key;
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
        
        public static string GenerateThumbnail(this IBlobService blobService, string key)
        {
            return blobService.GenerateSize(key, ImageSize.Thumb);
        }

        public static string GenerateSize(this IBlobService blobService, string key, ImageSize size, Transform transform = null, bool alwaysCreate = false)
        {
            if (transform == null)
                transform = WebCatalog.ImageSizes[size];
            
            var resizeKey = WebPath.Resize(size, key);
            
            if (!alwaysCreate && blobService.Exists(resizeKey))
                return resizeKey;
            
            using (var stream = blobService.Stream(key)) 
            using (var originalImage = (Bitmap)Bitmap.FromStream(stream))
            using (var resizedImage = transform.Mask ? 
                originalImage.Mask(transform.Width, transform.Height) : 
                originalImage.BoundingBox(transform.Width, transform.Height))
            {
                blobService.SaveImage(resizedImage, resizeKey);
            }

            return resizeKey;
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
            var thumbnailKey = WebPath.Resize(ImageSize.Thumb, key);
            return blobService.Exists(thumbnailKey);
        }
    }
}
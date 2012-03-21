﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Instatus.Services;
using System.Web.Helpers;
using Instatus.Web;

namespace Instatus.Services
{
    public interface IBlobService
    {
        string Save(string contentType, string slug, Stream stream);
        Stream Stream(string key);
        string[] Query(string folder);
    }
}

namespace Instatus
{
    public static class BlobServiceExtensions
    {
        public static void GenerateThumbnail(this IBlobService blobService, string key, int boundingBoxSize = 200, bool alwaysCreate = false) 
        {
            if (!alwaysCreate && blobService.HasThumbnail(key))
                return;
            
            var thumbnailKey = WebPath.Resize(WebSize.Thumb, key);

            using(var stream = blobService.Stream(key)) 
            {
                var webImage = new WebImage(stream);

                webImage.BoundingBox(200);

                blobService.Save("image/jpeg", thumbnailKey, webImage.GetJpgAsStream());
            }
        }

        public static bool BlobExists(this IBlobService blobService, string key) 
        {
            using(var stream = blobService.Stream(key)) 
            {
                return stream != null;
            }
        }

        public static bool HasThumbnail(this IBlobService blobService, string key) 
        {
            var thumbnailKey = WebPath.Resize(WebSize.Thumb, key);
            return blobService.BlobExists(thumbnailKey);
        }
    }
}
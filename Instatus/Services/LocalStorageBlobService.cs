using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.IO;
using System.Web.Hosting;
using Instatus.Web;
using System.Web.Helpers;
using Instatus.Data;

namespace Instatus.Services
{
    [Export(typeof(IBlobService))]
    public class LocalStorageBlobService : IBlobService
    {       
        public static string BasePath = "~/LocalStorage/";

        public static void EnsureFolderExists(string absolutePath)
        {
            var directoryName = Path.GetDirectoryName(absolutePath);
            
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
        }

        private string GetRelativePath(string contentType, string slug)
        {
            var extension = WebMimeType.GetExtension(contentType);
            var fileName = string.Format("{0}.{1}", slug ?? Generator.TimeStamp(), extension);
            return BasePath + fileName;
        }

        public string Save(string contentType, string slug, Stream stream)
        {
            var relativePath = GetRelativePath(contentType, slug);

            Save(relativePath, stream);

            return relativePath;
        }

        public static void Save(string relativePath, Stream stream) {
            var absolutePath = HostingEnvironment.MapPath(relativePath);

            EnsureFolderExists(absolutePath);

            using (var fs = new FileStream(absolutePath, FileMode.Create, FileAccess.Write))
            {
                stream.Flush();
                stream.Position = 0;
                stream.CopyTo(fs);
            }
        }

        public Stream Stream(string key)
        {
            return new FileStream(HostingEnvironment.MapPath(key), FileMode.Open, FileAccess.Read);
        }
    }
}
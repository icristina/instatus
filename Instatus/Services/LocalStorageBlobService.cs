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

        private void EnsureFolderExists()
        {
            var folderPath = HostingEnvironment.MapPath(BasePath);
            
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
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
            var absolutePath = HostingEnvironment.MapPath(relativePath);

            EnsureFolderExists();

            using (var fs = new FileStream(absolutePath, FileMode.Create, FileAccess.Write))
            {
                stream.Flush();
                stream.Position = 0;                
                stream.CopyTo(fs);
            }

            return relativePath;
        }

        public Stream Stream(string key)
        {
            return new FileStream(HostingEnvironment.MapPath(key), FileMode.Open, FileAccess.Read);
        }
    }
}
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

        public string Save(string contentType, string slug, Stream stream)
        {
            var extension = WebContentTypes.GetExtension(contentType);
            var fileName = string.Format("{0}.{1}", slug ?? Generator.TimeStamp(), extension);
            var relativePath = BasePath + fileName;
            var absolutePath = HostingEnvironment.MapPath(relativePath);
            var folderPath = HostingEnvironment.MapPath(BasePath);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            using (var fs = new FileStream(absolutePath, FileMode.Create, FileAccess.Write))
            {
                stream.Flush();
                stream.Position = 0;                
                stream.CopyTo(fs);
            }

            return relativePath;
        }
    }
}
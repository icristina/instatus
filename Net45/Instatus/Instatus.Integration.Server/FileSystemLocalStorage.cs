using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Instatus.Core;
using Instatus.Core.Utils;
using Instatus.Core.Extensions;

namespace Instatus.Integration.Server
{
    public class FileSystemLocalStorage : ILocalStorage
    {
        private IHosting hosting;
        
        public void Save(string virtualPath, Stream inputStream)
        {
            var absolutePath = MapPath(virtualPath);

            using (var fileStream = new FileStream(absolutePath, FileMode.Create, FileAccess.Write))
            {
                inputStream.Flush();
                inputStream.ResetPosition();
                inputStream.CopyTo(fileStream);
            }
        }

        public void Stream(string virtualPath, Stream outputStream)
        {
            var absolutePath = MapPath(virtualPath);

            using (var fileStream = new FileStream(absolutePath, FileMode.Open, FileAccess.Read))
            {
                fileStream.CopyTo(outputStream);
            }
        }

        public void Delete(string virtualPath)
        {            
            File.Delete(MapPath(virtualPath));
        }

        public bool EnableSubFolders { get; set; }

        public string MapPath(string virtualPath)
        {
            var subPath = EnableSubFolders ? 
                virtualPath.TrimStart(PathBuilder.RelativeChars) : 
                Path.GetFileName(virtualPath);
            
            return Path.Combine(hosting.RootPath, subPath);
        }

        public FileSystemLocalStorage(IHosting hosting)
        {
            this.hosting = hosting;

            EnableSubFolders = true;
        }
    }
}

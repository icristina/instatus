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
        
        public Stream OpenWrite(string virtualPath)
        {
            var absolutePath = MapPath(virtualPath);

            return new FileStream(absolutePath, FileMode.Create, FileAccess.Write);
        }

        public Stream OpenRead(string virtualPath)
        {
            var absolutePath = MapPath(virtualPath);

            return new FileStream(absolutePath, FileMode.Open, FileAccess.Read);
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

        public string[] Query(string virtualPath, string suffix)
        {
            var absolutePath = MapPath(virtualPath);

            return Directory.EnumerateFiles(absolutePath)
                .Where(p => p.EndsWith(suffix))
                .Select(p => p.Replace(absolutePath, virtualPath))
                .ToArray();
        }
        
        public FileSystemLocalStorage(IHosting hosting)
        {
            this.hosting = hosting;

            EnableSubFolders = true;
        }
    }
}

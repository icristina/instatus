using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Instatus.Core;

namespace Instatus.Integration.Server
{
    public class FileSystemLocalStorage : ILocalStorage
    {
        private IHostingEnvironment hostingEnvironment;
        
        public void Save(string virtualPath, Stream inputStream)
        {
            var absolutePath = MapPath(virtualPath);

            using (var fileStream = new FileStream(absolutePath, FileMode.Create, FileAccess.Write))
            {
                inputStream.Flush();
                inputStream.Position = 0;
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

        public string MapPath(string virtualPath)
        {
            var fileName = Path.GetFileName(virtualPath);
            var outputPath = hostingEnvironment.OutputPath;

            return Path.Combine(outputPath, fileName);
        }

        public FileSystemLocalStorage(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
    }
}

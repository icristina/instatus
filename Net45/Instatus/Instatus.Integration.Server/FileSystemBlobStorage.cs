using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;
using Instatus.Core.Utils;
using Instatus.Core.Models;

namespace Instatus.Integration.Server
{
    public class FileSystemBlobStorage : IBlobStorage
    {
        private FileSystemLocalStorage fileSystemLocalStorage;
        private IHosting hosting;
        
        public void Upload(string virtualPath, Stream inputStream, Metadata metaData)
        {
            using (var fileStream = fileSystemLocalStorage.OpenWrite(virtualPath))
            {
                inputStream.CopyTo(fileStream);
            }
        }

        public void Download(string virtualPath, Stream outputStream)
        {
            using (var fileStream = fileSystemLocalStorage.OpenRead(virtualPath))
            {
                fileStream.CopyTo(outputStream);
            }
        }

        public async void Copy(string virtualPath, string uri, Metadata metaData)
        {
            var absolutePath = fileSystemLocalStorage.MapPath(virtualPath);

            using (var httpClient = new HttpClient())
            using (var inputStream = await httpClient.GetStreamAsync(uri))
            {
                Upload(virtualPath, inputStream, metaData);
            }
        }

        public string GenerateSignedUrl(string virtualPath, string httpMethod)
        {
            throw new NotImplementedException();
        }

        public string MapPath(string virtualPath)
        {
            var baseUri = new Uri(hosting.BaseAddress);
            var absolutePath = virtualPath.TrimStart(PathBuilder.RelativeChars);
            
            return new Uri(baseUri, absolutePath).ToString();
        }

        public string[] Query(string virtualPath)
        {
            var outputPath = hosting.RootPath;
            var directoryPath = Path.Combine(outputPath, virtualPath.TrimStart(PathBuilder.RelativeChars));
            var files = Directory.GetFiles(directoryPath);

            return files.Select(file => {
                return virtualPath.TrimEnd(PathBuilder.DelimiterChars) + "/" + Path.GetFileName(file);
            })
            .ToArray();
        }

        public FileSystemBlobStorage(IHosting hosting)
        {
            this.hosting = hosting;
            this.fileSystemLocalStorage = new FileSystemLocalStorage(hosting)
            {
                EnableSubFolders = true
            };
        }
    }
}

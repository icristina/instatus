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

namespace Instatus.Integration.Server
{
    public class FileSystemBlobStorage : IBlobStorage
    {
        private FileSystemLocalStorage fileSystemLocalStorage;
        private IHostingEnvironment hostingEnvironment;
        
        public void Upload(string virtualPath, Stream inputStream, IMetadata metaData)
        {
            fileSystemLocalStorage.Save(virtualPath, inputStream);
        }

        public void Download(string virtualPath, Stream outputStream)
        {
            fileSystemLocalStorage.Stream(virtualPath, outputStream);
        }

        public async void Copy(string virtualPath, string uri, IMetadata metaData)
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
            var baseUri = new Uri(hostingEnvironment.BaseUri);
            var absolutePath = virtualPath.TrimStart(PathBuilder.RelativeChars);
            
            return new Uri(baseUri, absolutePath).ToString();
        }

        public string[] Query(string virtualPath)
        {
            var outputPath = hostingEnvironment.OutputPath;
            var directoryPath = Path.Combine(outputPath, virtualPath.TrimStart(PathBuilder.RelativeChars));
            var files = Directory.GetFiles(directoryPath);

            return files.Select(file => {
                return virtualPath.TrimEnd(PathBuilder.DelimiterChars) + "/" + Path.GetFileName(file);
            })
            .ToArray();
        }

        public FileSystemBlobStorage(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.fileSystemLocalStorage = new FileSystemLocalStorage(hostingEnvironment)
            {
                EnableSubFolders = true
            };
        }
    }
}

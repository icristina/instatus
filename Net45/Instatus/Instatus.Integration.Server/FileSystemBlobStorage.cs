using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;

namespace Instatus.Integration.Server
{
    public class FileSystemBlobStorage : IBlobStorage
    {
        private FileSystemLocalStorage fileSystemLocalStorage = new FileSystemLocalStorage();
        private IHostingEnvironment hostingEnvironment;
        
        public void Upload(string virtualPath, Stream inputStream, IMetadata metaData)
        {
            fileSystemLocalStorage.Save(virtualPath, inputStream);
        }

        public void Download(string virtualPath, Stream outputStream)
        {
            fileSystemLocalStorage.Stream(virtualPath, outputStream);
        }

        public void Copy(string virtualPath, string uri, IMetadata metaData)
        {
            var absolutePath = fileSystemLocalStorage.MapPath(virtualPath);
            
            using (var webClient = new WebClient()) 
            {
                webClient.DownloadFile(uri, absolutePath);
            }  
        }

        public string GenerateSignedUrl(string virtualPath, string httpMethod)
        {
            throw new NotImplementedException();
        }

        public static readonly char[] RelativeChars = new char[] { '~', '/', '\\' };

        public string MapPath(string virtualPath)
        {
            var baseUri = new Uri(hostingEnvironment.BaseUrl);
            var absolutePath = virtualPath.TrimStart(RelativeChars);
            
            return new Uri(baseUri, absolutePath).ToString();
        }

        public FileSystemBlobStorage(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
    }
}

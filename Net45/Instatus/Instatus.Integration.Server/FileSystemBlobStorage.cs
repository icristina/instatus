using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;

namespace Instatus.Integration.Server
{
    public class FileSystemBlobStorage : IBlobStorage
    {
        private FileSystemLocalStorage fileSystemLocalStorage;
        private IHostingEnvironment hostingEnvironment;
        
        public void Upload(string virtualPath, Stream inputStream, IMetadata metaData)
        {
            var absolutePath = MapLocalPath(virtualPath);

            using (var fileStream = new FileStream(absolutePath, FileMode.Create, FileAccess.Write))
            {
                inputStream.Flush();
                inputStream.Position = 0;
                inputStream.CopyTo(fileStream);
            }
        }

        public void Download(string virtualPath, Stream outputStream)
        {
            var absolutePath = MapLocalPath(virtualPath);

            using (var fileStream = new FileStream(absolutePath, FileMode.Open, FileAccess.Read))
            {
                fileStream.CopyTo(outputStream);
            }
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

        public static readonly char[] RelativeChars = new char[] { '~', '/', '\\' };
        public static readonly char[] PathDelimiterChars = new char[] { '/', '\\' };

        public string MapPath(string virtualPath)
        {
            var baseUri = new Uri(hostingEnvironment.BaseUri);
            var absolutePath = virtualPath.TrimStart(RelativeChars);
            
            return new Uri(baseUri, absolutePath).ToString();
        }

        public string[] Query(string virtualPath)
        {
            var outputPath = hostingEnvironment.OutputPath;
            var directoryPath = Path.Combine(outputPath, virtualPath.TrimStart(RelativeChars));
            var files = Directory.GetFiles(directoryPath);

            return files.Select(file => {
                return virtualPath.TrimEnd(PathDelimiterChars) + "/" + Path.GetFileName(file);
            })
            .ToArray();
        }

        public string MapLocalPath(string virtualPath)
        {
            var outputPath = hostingEnvironment.OutputPath;

            return Path.Combine(outputPath, virtualPath.TrimStart(RelativeChars));
        }

        public FileSystemBlobStorage(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.fileSystemLocalStorage =  new FileSystemLocalStorage(hostingEnvironment);
        }
    }
}

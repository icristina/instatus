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
        private IHosting hosting;
        
        public Stream OpenWrite(string virtualPath, Metadata metaData)
        {
            return new FileStream(MapPath(virtualPath), FileMode.Create, FileAccess.Write);
        }

        public Stream OpenRead(string virtualPath)
        {
            return new FileStream(MapPath(virtualPath), FileMode.Open, FileAccess.Read);
        }

        public async void Copy(string virtualPath, string uri, Metadata metaData)
        {
            using (var httpClient = new HttpClient())
            using (var outputStream = OpenWrite(virtualPath, metaData))
            using (var inputStream = await httpClient.GetStreamAsync(uri))
            {
                inputStream.CopyTo(outputStream);
            }
        }

        public string GenerateUri(string virtualPath, HttpMethod httpMethod)
        {
            var baseUri = new Uri(hosting.BaseAddress);
            var absolutePath = virtualPath.TrimStart(PathBuilder.RelativeChars);
            
            return new Uri(baseUri, absolutePath).ToString();
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
                .Where(p => string.IsNullOrEmpty(suffix) || p.EndsWith(suffix))
                .Select(p => p.Replace(absolutePath, virtualPath))
                .ToArray();
        }

        public void Delete(string virtualPath)
        {
            File.Delete(MapPath(virtualPath));
        }

        public FileSystemBlobStorage(IHosting hosting)
        {
            this.hosting = hosting;
            this.EnableSubFolders = true;
        }
    }
}

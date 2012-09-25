using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IBlobStorage : IVirtualPathUtility
    {
        void Upload(string virtualPath, Stream inputStream, Metadata metaData);
        void Download(string virtualPath, Stream outputStream);
        void Copy(string virtualPath, string uri, Metadata metaData);
        string GenerateSignedUrl(string virtualPath, string httpMethod);
        string[] Query(string virtualPath);
    }
}

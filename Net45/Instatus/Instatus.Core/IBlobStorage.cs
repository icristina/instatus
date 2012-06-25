using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IBlobStorage
    {
        void Upload(string virtualPath, Stream inputStream, IMetadata metaData);
        void Download(string virtualPath, Stream outputStream);
        void Copy(string virtualPath, string uri, IMetadata metaData);
        string GenerateSignedUrl(string virtualPath, string httpMethod);
        string MapPath(string virtualPath);
    }
}

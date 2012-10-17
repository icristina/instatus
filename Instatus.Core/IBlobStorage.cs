using Instatus.Core.Models;
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
        Stream OpenWrite(string virtualPath, Metadata metaData);
        Stream OpenRead(string virtualPath);
        void Copy(string virtualPath, string uri, Metadata metaData);
        string[] Query(string virtualPath, string suffix);
        string GenerateUri(string virtualPath, string httpMethod);
        void Delete(string virtualPath);
    }
}

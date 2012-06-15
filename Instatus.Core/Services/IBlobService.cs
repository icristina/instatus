using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Models;

namespace Instatus.Services
{
    public interface IBlobService
    {
        string Save(string contentType, string virtualPath, Stream stream);
        Stream Stream(string virtualPath);
        string[] Query();
        string MapPath(string virtualPath);
        Request GenerateSignedRequest(string contentType, string virtualPath);
        void Copy(string virtualPath, string uri);
    }
}

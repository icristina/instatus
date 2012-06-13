using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Instatus.Services
{
    public interface IBlobService
    {
        string Save(string contentType, string slug, Stream stream);
        Stream Stream(string key);
        string[] Query();
        string MapPath(string virtualPath);
    }
}

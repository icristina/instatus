using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Instatus.Services
{
    public interface IBlobService
    {
        string Save(string contentType, string slug, Stream stream);
        Stream Stream(string key);
    }
}
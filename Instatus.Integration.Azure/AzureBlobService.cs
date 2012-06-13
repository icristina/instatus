using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Services;

namespace Instatus.Integration.Azure
{
    public class AzureBlobService : IBlobService
    {
        public string[] Query()
        {
            throw new NotImplementedException();
        }

        public string Save(string contentType, string slug, Stream stream)
        {
            throw new NotImplementedException();
        }

        public Stream Stream(string key)
        {
            throw new NotImplementedException();
        }

        public string MapPath(string virtualPath)
        {
            throw new NotImplementedException();
        }
    }
}

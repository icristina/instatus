using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Models;
using Instatus.Services;

namespace Instatus.Integration.Azure
{
    public class AzureLocalStorageService : ILocalStorageService
    {
        public string MapPath(string virtualPath)
        {
            throw new NotImplementedException();
        }

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

        public Request GenerateSignedRequest(string contentType, string virtualPath)
        {
            throw new NotImplementedException();
        }
    }
}

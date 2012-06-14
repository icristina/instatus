using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Models;
using Instatus.Services;

namespace Instatus.Integration.Amazon
{
    // http://docs.amazonwebservices.com/sdkfornet/latest/apidocs/Index.html
    public class AmazonBlobService : IBlobService
    {
        private ICredentialService credentialService;
        private ICredential credential;

        private ICredential Credential
        {
            get
            {
                if (credential == null)
                    credential = credentialService.GetCredential(Provider.AmazonS3);

                return credential;
            }
        }        
        
        public string[] Query()
        {
            throw new NotImplementedException();
        }

        public string Save(string contentType, string virtualPath, Stream stream)
        {
            throw new NotImplementedException();
        }

        public Stream Stream(string virtualPath)
        {
            throw new NotImplementedException();
        }

        public string MapPath(string virtualPath)
        {
            throw new NotImplementedException();
        }

        // http://docs.amazonwebservices.com/AmazonS3/latest/dev/HTTPPOSTForms.html
        public Request GenerateSignedRequest(string contentType, string virtualPath)
        {
            var resource = new Resource(virtualPath);
            var policy = new
            {
                expiration = DateTime.UtcNow.AddHours(1),
                conditions = new object[] {
                    new {
                        bucket = resource.Bucket
                    },
                    new string[] {
                        "starts-with",
                        "$key",
                        resource.Key
                    },
                    new Dictionary<string, string>() {
                        { "Content-Type", contentType }
                    },
                    new string[] {
                        "content-length-range",
                        "0",
                        "1048576"
                    }
                }
            };         
            
            return null;
        }

        public AmazonBlobService(ICredentialService credentialService)
        {
            this.credentialService = credentialService;
        }
    }
}

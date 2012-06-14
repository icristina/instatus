using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus;
using Instatus.Models;
using Instatus.Services;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Instatus.Integration.Azure
{
    public class AzureBlobService : IBlobService
    {
        private ICredentialService credentialService;
        private ICredential credential;

        private ICredential Credential
        {
            get {
                if (credential == null)
                    credential = credentialService.GetCredential(Provider.AzureBlobStorage);

                return credential;
            }
        }
        
        public string[] Query()
        {
            throw new NotImplementedException();
        }

        private CloudBlob GetBlobReference(string virtualPath)
        {
            var resource = new Resource(virtualPath);
            var baseUri = string.Format("http://{0}.blob.core.windows.net", Credential.Alias);
            var client = new CloudBlobClient(baseUri, new StorageCredentialsAccountAndKey(Credential.Alias, Credential.Key));
            var container = client.GetContainerReference(resource.Bucket);

            return container.GetBlobReference(resource.Key);
        }

        public string Save(string contentType, string virtualPath, Stream stream)
        {
            var blob = GetBlobReference(virtualPath);

            blob.Properties.ContentType = contentType;
            blob.UploadFromStream(stream);

            return blob.Uri.ToString();
        }

        public Stream Stream(string key)
        {
            var stream = new MemoryStream();
            var blob = GetBlobReference(key);
            blob.DownloadToStream(stream);
            return stream;
        }

        public string MapPath(string virtualPath)
        {
            return GetBlobReference(virtualPath).Uri.ToString();
        }

        public Request GenerateSignedRequest(string contentType, string virtualPath)
        {
            throw new NotImplementedException();
        }

        public AzureBlobService(ICredentialService credentialService)
        {
            this.credentialService = credentialService;
        }
    }
}

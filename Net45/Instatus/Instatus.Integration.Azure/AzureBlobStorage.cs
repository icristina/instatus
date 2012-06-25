using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Instatus.Integration.Azure
{
    public class AzureBlobStorage : IBlobStorage
    {
        public const string ProviderName = "AzureBlobStorage";
        
        private ICredentialStorage credentialStorage;
        private ICredential credential;

        public ICredential Credential
        {
            get
            {
                if (credential == null)
                    credential = credentialStorage.GetCredential(ProviderName);

                return credential;
            }
        }

        public static readonly char[] RelativeChars = new char[] { '~', '/', '\\' };

        public Tuple<string, string> ParseVirtualPath(string virtualPath)
        {
            return new Tuple<string, string>(
                Path.GetDirectoryName(virtualPath).TrimStart(RelativeChars),
                Path.GetFileName(virtualPath)
            );
        }

        public string GetBaseUri(string accountName)
        {
            return string.Format("http://{0}.blob.core.windows.net", accountName);
        }

        public CloudBlob GetBlobReference(string virtualPath)
        {
            var resource = ParseVirtualPath(virtualPath);
            var baseUri = GetBaseUri(Credential.AccountName);
            var storageCredential = new StorageCredentialsAccountAndKey(Credential.AccountName, Credential.PrivateKey);
            var client = new CloudBlobClient(baseUri, storageCredential);
            var container = client.GetContainerReference(resource.Item1);

            return container.GetBlobReference(resource.Item2);
        }

        public void SetMetadata(CloudBlob cloudBlob, IMetadata metaData)
        {
            if (metaData != null)
            {
                cloudBlob.Properties.ContentType = metaData.ContentType;
            }
        }

        public void Upload(string virtualPath, Stream inputStream, IMetadata metaData)
        {
            var cloudBlob = GetBlobReference(virtualPath);

            SetMetadata(cloudBlob, metaData);

            cloudBlob.UploadFromStream(inputStream);
        }

        public void Download(string virtualPath, Stream outputStream)
        {
            var blob = GetBlobReference(virtualPath);

            blob.DownloadToStream(outputStream);
        }

        public void Copy(string virtualPath, string uri, IMetadata metaData)
        {
            var cloudBlob = GetBlobReference(virtualPath);

            SetMetadata(cloudBlob, metaData);

            using (var webClient = new WebClient())
            {
                var byteArray = webClient.DownloadData(uri);
                cloudBlob.UploadByteArray(byteArray);
            }  
        }

        public string GenerateSignedUrl(string virtualPath, string httpMethod)
        {
            throw new NotImplementedException();
        }

        public string MapPath(string virtualPath)
        {
            return GetBaseUri(Credential.AccountName) + "/" + virtualPath.TrimStart(RelativeChars);
        }

        public AzureBlobStorage(ICredentialStorage credentialStorage)
        {
            this.credentialStorage = credentialStorage;
        }
    }
}

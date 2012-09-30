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
using Instatus.Core.Utils;
using Instatus.Core.Extensions;
using Instatus.Core.Models;

namespace Instatus.Integration.Azure
{
    public class AzureBlobStorage : IBlobStorage
    {
        private IKeyValueStorage<Credential> credentials;

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
            var credential = credentials.Get(WellKnown.Provider.WindowsAzure);
            var baseUri = GetBaseUri(credential.AccountName);
            var storageCredential = new StorageCredentialsAccountAndKey(credential.AccountName, credential.PrivateKey);
            var client = new CloudBlobClient(baseUri, storageCredential);
            var container = client.GetContainerReference(resource.Item1);

            return container.GetBlobReference(resource.Item2);
        }

        public void SetMetadata(CloudBlob cloudBlob, Metadata metaData)
        {
            if (metaData != null)
            {
                cloudBlob.Properties.ContentType = metaData.ContentType;
            }
        }

        public Stream OpenWrite(string virtualPath, Metadata metaData)
        {
            var cloudBlob = GetBlobReference(virtualPath);

            SetMetadata(cloudBlob, metaData);

            return cloudBlob.OpenWrite();
        }

        public Stream OpenRead(string virtualPath)
        {
            var blob = GetBlobReference(virtualPath);

            return blob.OpenRead();
        }

        public void Copy(string virtualPath, string uri, Metadata metaData)
        {
            var cloudBlob = GetBlobReference(virtualPath);

            SetMetadata(cloudBlob, metaData);

            using (var webClient = new WebClient())
            {
                var byteArray = webClient.DownloadData(uri);
                cloudBlob.UploadByteArray(byteArray); // TODO: transition to Copy operation in 1.7.1 SDK
            }  
        }

        public string GenerateUri(string virtualPath, string httpMethod)
        {
            var credential = credentials.Get(WellKnown.Provider.WindowsAzure);
            var baseAddress = GetBaseUri(credential.AccountName);

            return new PathBuilder(baseAddress)
                .Path(virtualPath)
                .ToProtocolRelativeUri();
        }

        public string[] Query(string virtualPath, string suffix)
        {
            return null;
        }

        public void Delete(string virtualPath)
        {

        }

        public AzureBlobStorage(IKeyValueStorage<Credential> credentials)
        {
            this.credentials = credentials;
        }
    }
}

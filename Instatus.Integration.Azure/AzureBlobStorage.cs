using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Blob;
using Instatus.Core.Utils;
using Instatus.Core.Extensions;
using Instatus.Core.Models;
using System.Net.Http;
using Microsoft.WindowsAzure.Storage.Auth;

namespace Instatus.Integration.Azure
{
    public class AzureBlobStorage : IBlobStorage
    {
        private ILookup<Credential> credentials;

        public string GetBlobName(string virtualPath)
        {
            return Path.GetFileName(virtualPath);
        }

        public string GetContainerName(string virtualPath)
        {
            return Path.GetDirectoryName(virtualPath)
                .TrimStart(PathBuilder.RelativeChars)
                .TrimEnd(PathBuilder.RelativeChars)
                .ToLower();
        }

        public string GetBaseUri(string accountName)
        {
            return string.Format("http://{0}.blob.core.windows.net", accountName);
        }

        public CloudBlockBlob GetBlockBlob(string virtualPath)
        {
            var container = GetBlobContainer(virtualPath);
            var blobName = GetBlobName(virtualPath);

            return container.GetBlockBlobReference(blobName);
        }

        public CloudBlobContainer GetBlobContainer(string virtualPath)
        {
            if (!virtualPath.Contains('.') && !virtualPath.EndsWith("/"))
            {
                virtualPath = virtualPath + "/"; // ensure virtualPath for folder still returns container name
            }

            var containerName = GetContainerName(virtualPath);
            var credential = credentials.Get(WellKnown.Provider.WindowsAzure);
            var baseUri = GetBaseUri(credential.AccountName);
            var storageCredential = new StorageCredentials(credential.AccountName, credential.PrivateKey);
            var client = new CloudBlobClient(new Uri(baseUri), storageCredential);
            
            return client.GetContainerReference(containerName);
        }

        public void SetMetadata(ICloudBlob cloudBlob, Metadata metaData)
        {
            if (metaData != null)
            {
                cloudBlob.Properties.ContentType = metaData.ContentType;
            }
        }

        public Stream OpenWrite(string virtualPath, Metadata metaData)
        {
            var blockBlob = GetBlockBlob(virtualPath);

            SetMetadata(blockBlob, metaData);

            return blockBlob.OpenWrite();
        }

        public Stream OpenRead(string virtualPath)
        {
            var blockBlob = GetBlockBlob(virtualPath);

            return blockBlob.OpenRead();
        }

        public void Copy(string virtualPath, string uri, Metadata metaData)
        {
            var blockBlob = GetBlockBlob(virtualPath);

            SetMetadata(blockBlob, metaData);

            blockBlob.StartCopyFromBlob(new Uri(uri));
        }

        public string GenerateUri(string virtualPath, HttpMethod httpMethod)
        {
            var credential = credentials.Get(WellKnown.Provider.WindowsAzure);
            var baseAddress = GetBaseUri(credential.AccountName);

            return new PathBuilder(baseAddress)
                .Path(virtualPath)
                .ToProtocolRelativeUri();
        }

        public string[] Query(string virtualPath, string suffix)
        {
            var container = GetBlobContainer(virtualPath);
            
            return container
                .ListBlobs()
                .Select(b => string.Format("~/{0}/{1}", container.Name, Path.GetFileName(b.Uri.AbsolutePath)))
                .ToArray();
        }

        public void Delete(string virtualPath)
        {
            // not implemented, intentionally currently only provider read and write
        }

        public AzureBlobStorage(ILookup<Credential> credentials)
        {
            this.credentials = credentials;
        }
    }
}

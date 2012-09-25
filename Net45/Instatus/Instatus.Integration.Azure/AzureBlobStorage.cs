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
        private ICredentialStorage credentialStorage;

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
            var credential = credentialStorage.GetCredential(WellKnown.Provider.WindowsAzure);
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

        public void Upload(string virtualPath, Stream inputStream, Metadata metaData)
        {
            var cloudBlob = GetBlobReference(virtualPath);

            SetMetadata(cloudBlob, metaData);

            inputStream.ResetPosition();

            cloudBlob.UploadFromStream(inputStream);
        }

        public void Download(string virtualPath, Stream outputStream)
        {
            var blob = GetBlobReference(virtualPath);

            blob.DownloadToStream(outputStream);
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

        public string GenerateSignedUrl(string virtualPath, string httpMethod)
        {
            throw new NotImplementedException();
        }

        public string MapPath(string virtualPath)
        {
            var credential = credentialStorage.GetCredential(WellKnown.Provider.WindowsAzure);
            var baseAddress = GetBaseUri(credential.AccountName);

            return new PathBuilder(baseAddress)
                .Path(virtualPath)
                .ToProtocolRelativeUri();
        }

        public string[] Query(string virtualPath)
        {
            throw new NotImplementedException();
        }

        public AzureBlobStorage(ICredentialStorage credentialStorage)
        {
            this.credentialStorage = credentialStorage;
        }
    }
}

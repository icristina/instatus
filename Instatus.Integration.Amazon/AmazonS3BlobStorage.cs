using Instatus.Core;
using Instatus.Core.Models;
using Instatus.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Aws = Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Threading.Tasks;

namespace Instatus.Integration.Amazon
{
    public class AmazonS3BlobStorage : IBlobStorage
    {        
        private ILookup<Credential> credentials;
        private Credential credential;
        private bool forceLowerCase = true;

        private Credential Credential
        {
            get 
            {
                return credential ?? (credential = credentials.Get(WellKnown.Provider.Amazon));
            }
        }

        private string BucketName
        {
            get 
            {
                return Credential.AccountName;
            }
        }

        private string BaseAddress
        {
            get
            {
                return string.Format("http://{0}.s3.amazonaws.com", BucketName);
            }
        }

        private string GetKeyName(string virtualPath)
        {
            virtualPath = new PathBuilder(null, forceLowerCase)
                .Path(virtualPath)
                .ToRelativePath();

            if (string.IsNullOrEmpty(virtualPath))
            {
                throw new Exception("Key required");
            }

            return virtualPath;
        }

        private AmazonS3 GetAmazonS3Client()
        {
            return Aws.AWSClientFactory.CreateAmazonS3Client(Credential.PublicKey, Credential.PrivateKey);
        }

        public Stream OpenWrite(string virtualPath, Metadata metaData)
        {     
            var keyName = GetKeyName(virtualPath);            
            var putRequest = new PutObjectRequest();
            var amazonS3 = GetAmazonS3Client();

            putRequest
                .WithBucketName(BucketName)
                .WithKey(keyName);

            SetMetadata(putRequest, metaData);            
            
            return new AmazonS3WriteStream(amazonS3, putRequest);
        }

        private void SetMetadata(PutObjectRequest putRequest, Metadata metaData)
        {
            if (metaData == null)
            {
                return;
            }

            if (metaData.PublicRead)
            {
                putRequest.WithCannedACL(S3CannedACL.PublicRead);
            }

            if (!string.IsNullOrEmpty(metaData.ContentType))
            {
                putRequest.WithContentType(metaData.ContentType);
            }

            foreach (var header in metaData.Headers)
            {
                putRequest.AddHeader(header.Key, header.Value);
            }
        }

        public Stream OpenRead(string virtualPath)
        {
            var keyName = GetKeyName(virtualPath);            
            var getRequest = new GetObjectRequest();

            getRequest
                .WithBucketName(BucketName)
                .WithKey(keyName);

            var memoryStream = new MemoryStream();

            using (var amazonS3 = GetAmazonS3Client())
            using (var getResponse = amazonS3.GetObject(getRequest))
            {
                getResponse.ResponseStream.CopyTo(memoryStream);

                return memoryStream;
            }
        }

        public void Copy(string virtualPath, string uri, Metadata metaData)
        {
            throw new NotImplementedException();
        }

        public string[] Query(string virtualPath, string suffix)
        {
            var listRequest = new ListObjectsRequest();
            var prefix = GetKeyName(virtualPath);

            listRequest
                .WithBucketName(BucketName)
                .WithPrefix(prefix);
            
            using (var amazonS3 = GetAmazonS3Client())
            using (var listResponse = amazonS3.ListObjects(listRequest))
            {
                return listResponse.S3Objects
                    .Where(o => !o.Key.EndsWith("/")) // ignore s3 folders
                    .Select(o => o.Key
                        .Replace(prefix, virtualPath) // original virtualPath prefix
                        .Replace("//", "/")) // double slash
                    .ToArray();
            }
        }

        public string GenerateUri(string virtualPath, HttpMethod httpMethod)
        {
            return new PathBuilder(BaseAddress, forceLowerCase)
                .Path(virtualPath)
                .ToProtocolRelativeUri();
        }

        public void Delete(string virtualPath)
        {
            throw new NotImplementedException();
        }

        public AmazonS3BlobStorage(ILookup<Credential> credentials)
        {
            this.credentials = credentials;
        }
    }

    internal class AmazonS3WriteStream : MemoryStream
    {
        private AmazonS3 amazonS3;
        private PutObjectRequest putRequest;

        protected override void Dispose(bool disposing)
        {          
            Position = 0;

            putRequest
                .WithAutoCloseStream(false)
                .WithInputStream(this);

            using (amazonS3)
            using (var putObjectResponse = amazonS3.PutObject(putRequest))
            {
                // dispose of all s3 resources
            }
            
            base.Dispose(disposing);
        }

        public AmazonS3WriteStream(AmazonS3 amazonS3, PutObjectRequest putRequest) 
        {
            this.amazonS3 = amazonS3;
            this.putRequest = putRequest;
        }
    }
}

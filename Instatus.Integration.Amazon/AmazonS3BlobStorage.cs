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

namespace Instatus.Integration.Amazon
{
    public class AmazonS3BlobStorage : IBlobStorage
    {
        private ILookup<Credential> credentials;

        public string GetKeyName(string virtualPath)
        {
            return Path.GetFileName(virtualPath);
        }

        public string GetBucketName(string virtualPath)
        {
            return Path.GetDirectoryName(virtualPath)
                .TrimStart(PathBuilder.RelativeChars)
                .TrimEnd(PathBuilder.RelativeChars)
                .ToLower();
        }

        public string GetBaseUri(string accountName)
        {
            return string.Format("http://{0}.s3.amazonaws.com", accountName);
        }

        public AmazonS3 GetAmazonS3Client()
        {
            var credential = credentials.Get(WellKnown.Provider.Amazon);
            return Aws.AWSClientFactory.CreateAmazonS3Client(credential.PublicKey, credential.PrivateKey);
        }

        public Stream OpenWrite(string virtualPath, Metadata metaData)
        {
            var bucketName = GetBucketName(virtualPath);
            var keyName = GetKeyName(virtualPath);
            var amazonS3 = GetAmazonS3Client();
            var memoryStream = new MemoryStream();
            var putRequest = new PutObjectRequest();
                
            putRequest
                .WithBucketName(bucketName)
                .WithKey(keyName)
                .WithInputStream(memoryStream);

            var putResponse = amazonS3.PutObject(putRequest);

            return memoryStream;
        }

        public Stream OpenRead(string virtualPath)
        {
            var bucketName = GetBucketName(virtualPath);
            var keyName = GetKeyName(virtualPath);
            var amazonS3 = GetAmazonS3Client();
            var memoryStream = new MemoryStream();
            var getRequest = new GetObjectRequest();

            getRequest
                .WithBucketName(bucketName)
                .WithKey(keyName);

            var getResponse = amazonS3.GetObject(getRequest);

            return getResponse.ResponseStream;
        }

        public void Copy(string virtualPath, string uri, Metadata metaData)
        {
            throw new NotImplementedException();
        }

        public string[] Query(string virtualPath, string suffix)
        {
            throw new NotImplementedException();
        }

        public string GenerateUri(string virtualPath, HttpMethod httpMethod)
        {
            var credential = credentials.Get(WellKnown.Provider.Amazon);
            var baseAddress = GetBaseUri(credential.AccountName);

            return new PathBuilder(baseAddress)
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
}

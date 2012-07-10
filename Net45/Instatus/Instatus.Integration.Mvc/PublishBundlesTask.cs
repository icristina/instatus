using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Optimization;
using Instatus.Core;
using Instatus.Core.Impl;

namespace Instatus.Integration.Mvc
{
    public class PublishBundlesTask : ITask
    {
        private IBlobStorage blobStorage;
        
        public void Process()
        {
            var bundleCollection = BundleTable.Bundles;
            
            foreach (var bundle in bundleCollection)
            {
                var httpContext = CreateHttpContextBase();
                var bundleContext = new BundleContext(httpContext, bundleCollection, bundle.Path);
                var bundleResponse = bundle.GenerateBundleResponse(bundleContext);

                using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(bundleResponse.Content)))
                {
                    var metadata = new BaseMetadata()
                    {
                        ContentType = bundleResponse.ContentType
                    };
                    
                    blobStorage.Upload(bundle.Path, memoryStream, metadata);
                }
            }
        }

        private HttpContextBase CreateHttpContextBase()
        {
            return new HttpContextWrapper(new HttpContext(
                new HttpRequest("", "http://localhost/", ""),
                new HttpResponse(new StringWriter())
            ));
        }

        public PublishBundlesTask(IBlobStorage blobStorage)
        {
            this.blobStorage = blobStorage;
        }
    }
}

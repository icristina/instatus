using Autofac;
using Autofac.Integration.Mvc;
using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Routing;
using Instatus.Core.Extensions;
using System.IO.Compression;

namespace Instatus.Integration.Mvc
{
    public class ImageHandler : IHttpHandler, IRouteHandler
    {
        private RequestContext requestContext;
        
        public bool IsReusable
        {
            get 
            {
                return false;
            }
        }

        public const string BucketParameterName = "bucket";
        public const string WidthParameterName = "width";
        public const string HeightParameterName = "height";
        public const string ActionParameterName = "action";
        public const string ResizeActionName = "r";
        public const string CoverActionName = "c";
        public const string ContainActionName = "f";

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            
            using (ILifetimeScope container = AutofacDependencyResolver.Current.ApplicationContainer.BeginLifetimeScope())
            {
                var blobStorage = container.Resolve<IBlobStorage>();
                var imaging = container.Resolve<IImaging>();
                var routeData = requestContext.RouteData.Values;
                var fileName = Path.GetFileName(request.Path);
                var bucketName = routeData.GetValue<string>(BucketParameterName);
                var width = routeData.GetValue<int>(WidthParameterName);
                var height = routeData.GetValue<int>(HeightParameterName);

                if (bucketName.Equals(fileName))
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return;
                }

                var virtualPath = string.Format("~/{0}/{1}", bucketName, fileName);

                if (width <= 0 || height <= 0)
                    throw new ArgumentOutOfRangeException();

                using (var inputMemoryStream = new MemoryStream())
                using (var outputMemoryStream = new MemoryStream())
                using (var gzipStream = new GZipStream(response.OutputStream, CompressionMode.Compress))
                {
                    try
                    {
                        blobStorage.Download(virtualPath, inputMemoryStream);
                    }
                    catch (FileNotFoundException exception)
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        return;
                    }

                    response.ContentType = "image/jpg";
                    response.AddHeader("Content-Encoding", "gzip");
                    response.ExpiresAbsolute = DateTime.UtcNow.AddDays(1);

                    inputMemoryStream.ResetPosition();
                    
                    switch (routeData.GetValue<string>(ActionParameterName))
                    {
                        case CoverActionName:
                            imaging.Cover(inputMemoryStream, outputMemoryStream, width, height);
                            break;
                        case ContainActionName:
                            imaging.Contain(inputMemoryStream, outputMemoryStream, width, height);
                            break;
                        default:
                            imaging.Resize(inputMemoryStream, outputMemoryStream, width, height);
                            break;
                    }

                    outputMemoryStream.ResetPosition();
                    outputMemoryStream.CopyTo(gzipStream);
                }                
            }
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            this.requestContext = requestContext;
            return this;
        }
    }
}

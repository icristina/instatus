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

        public T GetValue<T>(IDictionary<string, object> values, string key)
        {
            var output = values[key];

            if (output != null)
                return (T)Convert.ChangeType(output, typeof(T));

            return default(T);
        }

        public void ResetStream(Stream stream)
        {
            stream.Position = 0;
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
                var bucketName = GetValue<string>(routeData, BucketParameterName);
                var width = GetValue<int>(routeData, WidthParameterName);
                var height = GetValue<int>(routeData, HeightParameterName);

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
                    response.ExpiresAbsolute = DateTime.UtcNow.AddDays(1);

                    ResetStream(inputMemoryStream);
                    
                    switch (GetValue<string>(routeData, ActionParameterName))
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

                    ResetStream(outputMemoryStream);

                    outputMemoryStream.CopyTo(response.OutputStream);
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

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
        private IEnumerable<Tuple<int, int>> whiteListDimensions;

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
        
        public const int MinDimension = 16;
        public const int MaxDimension = 2000;

        public static readonly string[] WhiteListActions = new string[] { CoverActionName, ResizeActionName, ContainActionName };
        public static readonly string[] WhiteListExtensions = new string[] { ".jpg", ".gif", ".png", ".tif", ".bmp" };

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            
            using (ILifetimeScope container = AutofacDependencyResolver.Current.ApplicationContainer.BeginLifetimeScope())
            {
                var blobStorage = container.Resolve<IBlobStorage>();
                var imaging = container.Resolve<IImaging>();

                var fileName = Path.GetFileName(request.Path);
                var fileExtension = Path.GetExtension(request.Path);

                var routeData = requestContext.RouteData.Values;
                var action = routeData.GetValue<string>(ActionParameterName);
                var bucketName = routeData.GetValue<string>(BucketParameterName);
                var width = routeData.GetValue<int>(WidthParameterName);
                var height = routeData.GetValue<int>(HeightParameterName);
                var newDimensions = new Tuple<int, int>(width, height);
                var virtualPath = string.Format("~/{0}/{1}", bucketName, fileName);

                if (bucketName.Equals(fileName)
                    || width < MinDimension
                    || height < MinDimension
                    || width > MaxDimension
                    || height > MaxDimension
                    || (whiteListDimensions != null && !whiteListDimensions.Contains(newDimensions))
                    || !WhiteListActions.Contains(action)
                    || !WhiteListExtensions.Contains(fileExtension))
                {
                    NotFound(context);
                    return;
                }

                using (var inputMemoryStream = new MemoryStream())
                using (var outputMemoryStream = new MemoryStream())
                using (var gzipStream = new GZipStream(response.OutputStream, CompressionMode.Compress))
                {
                    try
                    {
                        blobStorage.Download(virtualPath, inputMemoryStream);

                        inputMemoryStream.ResetPosition();
                    
                        switch (action)
                        {
                            case ResizeActionName:
                                imaging.Resize(inputMemoryStream, outputMemoryStream, width, height);
                                break;
                            case ContainActionName:
                                imaging.Contain(inputMemoryStream, outputMemoryStream, width, height);
                                break;
                            case CoverActionName:
                                imaging.Cover(inputMemoryStream, outputMemoryStream, width, height);
                                break;
                        }
                    }
                    catch
                    {
                        NotFound(context);
                        return;
                    }

                    response.ContentType = WellKnown.ContentType.Jpg;
                    response.AddHeader("Content-Encoding", "gzip");
                    response.ExpiresAbsolute = DateTime.UtcNow.AddDays(1);

                    outputMemoryStream.ResetPosition();
                    outputMemoryStream.CopyTo(gzipStream);
                }                
            }
        }

        private void NotFound(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            this.requestContext = requestContext;
            return this;
        }

        public ImageHandler(IEnumerable<Tuple<int, int>> whiteListDimensions)
        {
            this.whiteListDimensions = whiteListDimensions;
        }
    }
}

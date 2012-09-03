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
    public class ImageHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get 
            {
                return true;
            }
        }

        public T GetValue<T>(NameValueCollection values, string key)
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

        public const string WidthParameterName = "w";
        public const string HeightParameterName = "h";
        public const string LeftParameterName = "x";
        public const string TopParameterName = "y";
        public const string ActionParameterName = "a";
        public const string CropActionName = "cr";
        public const string CoverActionName = "cv";
        public const string ContainActionName = "cn";

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            
            using (ILifetimeScope container = AutofacDependencyResolver.Current.ApplicationContainer.BeginLifetimeScope())
            {
                var blobStorage = container.Resolve<IBlobStorage>();
                var imaging = container.Resolve<IImaging>();

                var properties = request.QueryString;
                var fileName = Path.GetFileName(request.Path);
                var width = GetValue<int>(properties, WidthParameterName);
                var height = GetValue<int>(properties, HeightParameterName);

                if (width <= 0 || height <= 0)
                    throw new ArgumentOutOfRangeException();

                using (var inputMemoryStream = new MemoryStream())
                using (var outputMemoryStream = new MemoryStream())
                {
                    try
                    {
                        blobStorage.Download("~/media/" + fileName, inputMemoryStream);
                    }
                    catch (FileNotFoundException exception)
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        return;
                    }

                    response.ContentType = "image/jpg";
                    response.ExpiresAbsolute = DateTime.UtcNow.AddDays(1);

                    ResetStream(inputMemoryStream);
                    
                    switch (GetValue<string>(properties, ActionParameterName))
                    {
                        case CropActionName:
                            var left = GetValue<int>(properties, LeftParameterName);
                            var top = GetValue<int>(properties, TopParameterName);
                            imaging.Crop(inputMemoryStream, outputMemoryStream, left, top, width, height);
                            break;
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
    }

    public class ImageHandlerRoute : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ImageHandler();
        }
    }
}

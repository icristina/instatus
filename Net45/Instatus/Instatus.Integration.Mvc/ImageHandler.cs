using Autofac;
using Autofac.Integration.Mvc;
using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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

        public void ProcessRequest(HttpContext context)
        {
            using (ILifetimeScope container = AutofacDependencyResolver.Current.ApplicationContainer.BeginLifetimeScope())
            {
                var blobStorage = container.Resolve<IBlobStorage>();
                var imaging = container.Resolve<IImaging>();

                var properties = context.Request.QueryString;
                var fileName = Path.GetFileName(context.Request.Path);
                var width = GetValue<int>(properties, "width");
                var height = GetValue<int>(properties, "height");

                if (width <= 0 || height <= 0)
                    throw new ArgumentOutOfRangeException();

                context.Response.ContentType = "image/jpg";
                context.Response.ExpiresAbsolute = DateTime.UtcNow.AddDays(1);

                using (var inputMemoryStream = new MemoryStream())
                using (var outputMemoryStream = new MemoryStream())
                {
                    blobStorage.Download("~/media/" + fileName, inputMemoryStream);

                    ResetStream(inputMemoryStream);
                    
                    switch (GetValue<string>(properties, "filter")) 
                    {
                        case "crop":
                            var left = GetValue<int>(properties, "left");
                            var top = GetValue<int>(properties, "top");
                            imaging.Crop(inputMemoryStream, outputMemoryStream, left, top, width, height);
                            break;
                        case "cover":
                            imaging.Cover(inputMemoryStream, outputMemoryStream, width, height);
                            break;
                        case "contain":
                            imaging.Contain(inputMemoryStream, outputMemoryStream, width, height);
                            break;
                        default:
                            imaging.Resize(inputMemoryStream, outputMemoryStream, width, height);
                            break;
                    }

                    ResetStream(outputMemoryStream);

                    outputMemoryStream.CopyTo(context.Response.OutputStream);
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

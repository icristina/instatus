using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Runtime.Serialization;
using System.IO;
using System.Web.Hosting;
using System.Collections;
using System.Data;

namespace Instatus
{
    public static class RequestExtensions
    {
        public static string BaseUri(this HttpRequest request)
        {
            return request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath.TrimEnd('/');
        }

        public static string BaseUri(this HttpRequestBase request)
        {
            return HttpContext.Current.Request.BaseUri();
        }

        public static string Body(this HttpRequestBase request)
        {
            request.InputStream.Position = 0;
            return new StreamReader(request.InputStream).ReadToEnd();
        }
    }
}
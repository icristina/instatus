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
using Instatus.Web;

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

        public static bool HasFile(this HttpRequestBase request, WebContentType? contentType = null)
        {
            return request.Files.Count > 0 
                && request.Files[0].ContentLength > 0
                && (!contentType.HasValue || contentType.Value.IsContentType(request.Files[0].ContentType)); 
        }

        public static Stream FileInputStream(this HttpRequestBase request)
        {
            return request.Files[0].InputStream;
        }

        public static void RedirectToHome(this HttpResponseBase response)
        {
            response.Redirect("/");
        }
    }
}
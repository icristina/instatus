using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Hosting;
using Instatus;

namespace Instatus.Integration.Mvc
{
    // http://consultingblogs.emc.com/howardvanrooijen/archive/2009/08/25/cloaking-your-asp-net-mvc-web-application-on-iis-7.aspx
    public class FilterResponseHeadersModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public static readonly List<string> Headers = new List<string>()
        {
            "Server"
        };

        public void Init(HttpApplication context)
        {
            if (HttpRuntime.UsingIntegratedPipeline)       
                context.PreSendRequestHeaders += this.OnPreSendRequestHeaders;
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            if (HttpContext.Current != null)
                Headers.ForEach(h => HttpContext.Current.Response.Headers.Remove(h));
        }
    }
}
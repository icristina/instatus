using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Data;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Hosting;
using Instatus;

namespace Instatus.Web
{
    // http://consultingblogs.emc.com/howardvanrooijen/archive/2009/08/25/cloaking-your-asp-net-mvc-web-application-on-iis-7.aspx
    public class FilterResponseHeadersModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public static List<string> Headers = new List<string>()
                                        {
                                                "Server",
                                                "X-AspNet-Version",
                                                "X-AspNetMvc-Version",
                                                "X-Powered-By",
                                        };

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += this.OnPreSendRequestHeaders;
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            Headers.ForEach(h => HttpContext.Current.Response.Headers.Remove(h));
        }
    }
}
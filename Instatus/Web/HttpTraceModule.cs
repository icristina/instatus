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
    public class HttpTraceModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public static List<Regex> Patterns = new List<Regex>();

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.Check);
        }

        private void Check(Object source, EventArgs e)
        {
            foreach(var expression in Patterns) {
                var request = HttpContext.Current.Request;
                var uri = request.Url.AbsoluteUri;
                
                if(expression.Match(uri).Success) {
                    var filePath = HostingEnvironment.MapPath("~/App_Data/RequestTrace.txt");

                    if (request.ContentLength > 0)
                    {
                        File.AppendAllText(filePath, string.Format("{1} ({2}) {3}{0}{4}{0}{0}", Environment.NewLine, request.HttpMethod, request.ContentType, uri, request.InputStream.CopyToString()));
                    }
                    else
                    {
                        File.AppendAllText(filePath, string.Format("{1} ({2}) {3}{0}{0}", Environment.NewLine, request.HttpMethod, request.ContentType, uri));
                    }

                    break;
                }
            }
        }
    }
}
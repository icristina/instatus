using Instatus.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Instatus.Core.Extensions;

namespace Instatus.Integration.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Submit<T>(this HtmlHelper<T> htmlHelper, string label = "Submit", string className = "btn btn-primary")
        {
            var tagBuilder = new TagBuilder("button");

            tagBuilder.Attributes.Add("type", "submit");
            tagBuilder.Attributes.Add("class", className);
            tagBuilder.InnerHtml = label;

            return new MvcHtmlString(tagBuilder.ToString());
        }        
        
        public static MvcHtmlString BrowserCapabilitiesHint<T>(this HtmlHelper<T> htmlHelper)
        {
            var browserCapabilities = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Browser;
            var userAgent = htmlHelper.ViewContext.HttpContext.Request.UserAgent;
            var sb = new StringBuilder();

            sb.Append("no-js ");
            sb.Append(browserCapabilities.Browser.ToLower());
            sb.Append(" ");
            sb.Append(browserCapabilities.Browser.ToLower());
            sb.Append(browserCapabilities.MajorVersion);
            
            var device = "desktop";
            var isMobile = userAgent.Contains("Mobile");

            if (userAgent.Contains("iPad") || (userAgent.Contains("Android") && !isMobile))
            {
                device = "tablet";
            }
            else if (isMobile)
            {
                device = "mobile";
            }

            sb.Append(" ");
            sb.Append(device);

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString DataSource(string variableName, object graph)
        {
            var jsonSerializer = DependencyResolver.Current.GetService<IJsonSerializer>();
            var html = string.Format("<script>window.{0} = {1};</script>", variableName, jsonSerializer.Stringify(graph));

            return new MvcHtmlString(html);
        }
    }
}

using Instatus.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString BrowserCapabilitiesHint<T>(this HtmlHelper<T> htmlHelper)
        {
            var browserCapabilities = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Browser;
            var sb = new StringBuilder();

            sb.Append("no-js ");
            sb.Append(browserCapabilities.Browser.ToLower());
            sb.Append(" ");
            sb.Append(browserCapabilities.Browser.ToLower());
            sb.Append(browserCapabilities.MajorVersion);

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString InlineData(string variableName, object graph)
        {
            var jsonSerializer = DependencyResolver.Current.GetService<IJsonSerializer>();
            var html = string.Format("<script>window.{0} = {1};</script>", variableName, jsonSerializer.Stringify(graph));

            return new MvcHtmlString(html);
        }
    }
}

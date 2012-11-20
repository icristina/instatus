using Instatus.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Instatus.Core.Extensions;
using System.Web.WebPages;

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
            var browser = browserCapabilities.Browser.ToLower();
            var displayModeProvider = DisplayModeProvider.Instance;
            var sb = new StringBuilder();

            sb.Append("no-js ");
            sb.Append(browser);
            sb.Append(" ");
            sb.Append(browser);
            sb.Append(browserCapabilities.MajorVersion);
            sb.Append(" ");
            sb.Append(displayModeProvider.GetAvailableDisplayModesForContext(htmlHelper.ViewContext.HttpContext, htmlHelper.ViewContext.DisplayMode).FirstOrDefault().DisplayModeId.ToLower());            

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString RouteHint<T>(this HtmlHelper<T> htmlHelper)
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var controllerName = routeData.GetRequiredString("controller").ToLower();
            var actionName = routeData.GetRequiredString("action").ToLower();

            return new MvcHtmlString(string.Format("{0} {1}", controllerName, actionName));
        }

        public static MvcHtmlString DataSource(string variableName, object graph)
        {
            var jsonSerializer = DependencyResolver.Current.GetService<IJsonSerializer>();
            var html = string.Format("<script>window.{0} = {1};</script>", variableName, jsonSerializer.Stringify(graph));

            return new MvcHtmlString(html);
        }

        // https://github.com/erichexter/twitter.bootstrap.mvc/blob/master/src/BootstrapSupport/ViewHelperExtensions.cs
        public static MvcHtmlString TryPartial<T>(this HtmlHelper<T> htmlHelper, string viewName, object model = null)
        {
            try
            {
                return htmlHelper.Partial(viewName, model);
            }
            catch (Exception)
            {
                // do nothing
            }

            return MvcHtmlString.Empty;
        }
    }
}

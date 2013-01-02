using Instatus.Core;
using Instatus.Core.Extensions;
using Instatus.Integration.Mvc;
using Instatus.Scaffold.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Instatus.Scaffold
{
    public static class HtmlHelperExtensions
    {       
        public static MvcHtmlString Pagination<T>(this HtmlHelper<T> htmlHelper, IPaged paged) 
        {
            return htmlHelper.Partial("_Pagination", paged);
        }

        public static MvcHtmlString FacebookScripts<T>(this HtmlHelper<T> htmlHelper)
        {
            return htmlHelper.Partial("_FacebookScripts");
        }

        public static MvcHtmlString GoogleAnalyticsScripts<T>(this HtmlHelper<T> htmlHelper)
        {
            return htmlHelper.Partial("_GoogleAnalyticsScripts");
        }

        public static MvcHtmlString SelectLocale<T>(this HtmlHelper<T> htmlHelper)
        {
            return htmlHelper.Partial("_SelectLocale");
        }

        public static MvcHtmlString Metadata<T>(this HtmlHelper<T> htmlHelper, IDictionary<string, object> metadata) 
        {
            return htmlHelper.Partial("_Metadata", metadata);
        }

        public static MvcHtmlString LoginStatus<T>(this HtmlHelper<T> htmlHelper)
        {
            return htmlHelper.Partial("_LoginStatus");
        }

        public static MvcHtmlString Actions<T>(this HtmlHelper<T> htmlHelper, object item)
        {
            return htmlHelper.Partial("_Actions", item);
        }

        public static MvcHtmlString Commands<T>(this HtmlHelper<T> htmlHelper)
        {
            return htmlHelper.Partial("_Commands");
        }

        public static MvcHtmlString AppBar<T>(this HtmlHelper<T> htmlHelper)
        {
            return htmlHelper.TryPartial("_AppBar");
        }

        public static MvcHtmlString ActionButton<T>(this HtmlHelper<T> htmlHelper, string text, string actionName, string controllerName = null, object routeData = null, string className = "btn btn-primary")
        {
            var button = new Button() 
            {
                Text = text,
                ActionName = actionName,
                ControllerName = controllerName ?? htmlHelper.ViewContext.RouteData.GetRequiredString("controller"),
                RouteData = routeData,
                ClassName = className
            };

            return htmlHelper.Partial("_Button", button);
        }
    }
}
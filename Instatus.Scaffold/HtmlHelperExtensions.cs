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
        public static MvcHtmlString RadioList<T>(this HtmlHelper<T> htmlHelper, string name, SelectList selectList)
        {
            var radioList = new RadioList()
            {
                Name = string.IsNullOrWhiteSpace(name) ? htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix : name,
                SelectList = selectList
            };
            
            return htmlHelper.Partial("_RadioList", radioList);
        }
        
        public static MvcHtmlString Pagination<T>(this HtmlHelper<T> htmlHelper, IPaged paged, object routeValues = null) 
        {
            var pagination = new Pagination()
            {
                List = paged,
                RouteValues = routeValues
            };
            
            return htmlHelper.Partial("_Pagination", pagination);
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

        public static MvcHtmlString Modal<T>(this HtmlHelper<T> htmlHelper)
        {
            return htmlHelper.Partial("_Modal");
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

        public static MvcHtmlString Page<T>(this HtmlHelper<T> htmlHelper, string key = "", string viewName = "Details", object variant = null)
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var segments = new List<string>();

            if (string.IsNullOrEmpty(key))
            {
                segments.Add(routeData.DataTokens["area"].AsString());
                segments.Add(routeData.GetRequiredString("controller"));
            }
            else
            {
                segments.Add(key);
            }

            if (variant != null)
            {
                segments.Add(variant.AsString());
            }

            key = string.Join("-", segments.Where(s => !string.IsNullOrEmpty(s)).ToArray()).ToLower();

            return htmlHelper.Action("Details", BaseMvcConfig.PageControllerName, new { key = key, area = string.Empty, viewName = viewName });
        }

        public static MvcHtmlString PageLink<T>(this HtmlHelper<T> htmlHelper, string linkText, string key, object htmlAttributes = null)
        {
            return htmlHelper.RouteLink(linkText, WellKnown.RouteName.Page, new { key = key }, htmlAttributes);
        }
    }
}
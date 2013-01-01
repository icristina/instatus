﻿using Instatus.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Instatus.Core.Extensions;
using System.Web.WebPages;
using System.Collections;

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
            var httpContext = htmlHelper.ViewContext.RequestContext.HttpContext;
            var browserCapabilities = httpContext.Request.Browser;
            var browser = browserCapabilities.Browser.ToLower();
            var displayModeProvider = DisplayModeProvider.Instance;
            var sb = new StringBuilder();

            sb.Append("no-js ");
            sb.Append(browser);
            sb.Append(" ");
            sb.Append(browser);
            sb.Append(browserCapabilities.MajorVersion);
            sb.Append(" ");
            sb.Append(displayModeProvider.GetDisplayModeId(httpContext).ToLower());            

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString RouteHint<T>(this HtmlHelper<T> htmlHelper)
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var controllerName = routeData.GetRequiredString("controller").ToLower();
            var actionName = routeData.GetRequiredString("action").ToLower();

            return new MvcHtmlString(string.Format("{0} {1}", controllerName, actionName));
        }

        public static MvcHtmlString PositionHint<T>(this HtmlHelper htmlHelper, IList<T> list, T item, string firstHint = "first", string lastHint = "last", string middleHint = "")
        {
            var sb = new StringBuilder();
            var index = list.IndexOf(item);

            if (index == 0)
            {
                sb.Append(firstHint);
            }
            else if (index == list.Count - 1)
            {
                sb.Append(lastHint);
            }
            else
            {
                sb.Append(middleHint);
            }

            if (index % 2 == 0)
            {
                sb.Append(" odd");
            }
            else
            {
                sb.Append(" even");
            }

            return new MvcHtmlString(sb.ToString().Trim());
        }

        public static MvcHtmlString ActiveHint<T>(this HtmlHelper<T> htmlHelper, string controllerName)
        {
            if (htmlHelper.ViewContext.RouteData.GetRequiredString("controller").Equals(controllerName, StringComparison.OrdinalIgnoreCase))
            {
                return new MvcHtmlString("active");
            }
            else
            {
                return MvcHtmlString.Empty;
            }
        }

        public static MvcHtmlString LayoutHint<T>(this HtmlHelper<T> htmlHelper, IDictionary<object, dynamic> pageData)
        {
            return new MvcHtmlString(htmlHelper.ViewBag.LayoutHint ?? pageData["LayoutHint"]);
        }

        public static MvcHtmlString ValidationHint<T>(this HtmlHelper<T> htmlHelper)
        {
            if (htmlHelper.ViewData.ModelMetadata.IsRequired)
            {
                return new MvcHtmlString("required");
            }
            else
            {
                return MvcHtmlString.Empty;
            }
        }

        public static MvcHtmlString TableHint<T>(this HtmlHelper<T> htmlHelper)
        {
            return new MvcHtmlString("table table-striped table-bordered table-condensed span12");
        }

        public static MvcHtmlString DataSource<T>(this HtmlHelper<T> htmlHelper, string variableName, object graph)
        {
            var jsonSerializer = DependencyResolver.Current.GetService<IJsonSerializer>();
            var html = string.Format("<script>window.{0} = {1};</script>", variableName, jsonSerializer.Stringify(graph));

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString TransformContent<T>(this HtmlHelper<T> htmlHelper, string content)
        {
            var transform = DependencyResolver.Current.GetService<ITransform<string>>();
            var html = transform.Transform(content);

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

        public static MvcHtmlString Title<T>(this HtmlHelper<T> htmlHelper)
        {
            var tagBuilder = new TagBuilder("title");
            var title = htmlHelper.ViewBag.Title ?? htmlHelper.ViewContext.RouteData.GetRequiredString("controller").ToTitleCase();

            tagBuilder.InnerHtml = title;

            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString RichText<T>(this HtmlHelper<T> htmlHelper, string richText)
        {
            if (string.IsNullOrWhiteSpace(richText)) 
            {
                return MvcHtmlString.Empty;
            }

            if (!richText.ContainsIgnoreCase("<p") && !richText.ContainsIgnoreCase("<div"))
            {
                richText = string.Format("<p>{0}</p>", richText);
            }

            return new MvcHtmlString(richText);           
        }
    }
}
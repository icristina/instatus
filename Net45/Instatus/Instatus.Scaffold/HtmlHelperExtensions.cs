﻿using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Instatus.Scaffold
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Submit<T>(this HtmlHelper<T> htmlHelper, string label = "Submit")
        {
            var tagBuilder = new TagBuilder("button");

            tagBuilder.Attributes.Add("type", "submit");
            tagBuilder.InnerHtml = label;
            
            return new MvcHtmlString(tagBuilder.ToString());
        }
        
        public static MvcHtmlString Pagination<T>(this HtmlHelper<T> htmlHelper, IPaged paged) 
        {
            return htmlHelper.Partial("_Pagination", paged);
        }

        public static MvcHtmlString Facebook<T>(this HtmlHelper<T> htmlHelper)
        {
            return htmlHelper.Partial("_Facebook");
        }

        public static MvcHtmlString GoogleAnalytics<T>(this HtmlHelper<T> htmlHelper)
        {
            return htmlHelper.Partial("_GoogleAnalytics");
        }

        public static MvcHtmlString SelectLocale<T>(this HtmlHelper<T> htmlHelper)
        {
            return htmlHelper.Partial("_SelectLocale");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Routing;
using Instatus.Models;
using Instatus.Web;
using Instatus.Services;
using System.IO;

namespace Instatus
{
    public static class HtmlHelperExtensions
    {
        public static string Layout<T>(this HtmlHelper<T> html, string virtualPath)
        {
            return html.ViewContext.IsChildAction || html.ViewContext.HttpContext.Request.IsAjaxRequest() ? null : WebPath.Relative(virtualPath);
        }        
        
        public static MvcHtmlString ReturnUrl<T>(this HtmlHelper<T> html, string returnUrl = null)
        {
            var request = html.ViewContext.RequestContext.HttpContext.Request;
            return html.Hidden(WebConstant.QueryParameter.ReturnUrl, returnUrl ?? request.Params[WebConstant.QueryParameter.ReturnUrl] ?? request.RawUrl);
        }
        
        public static MvcHtmlString Tag<T>(this HtmlHelper<T> html, string tagName, object value, string itemPropertyName = null)
        {
            if (value.IsEmpty())
                return null;

            var tag = new TagBuilder(tagName);

            tag.MergeAttributeOrEmpty("itemprop", itemPropertyName);
            tag.SetInnerText(value.ToString());
            
            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString Scripts<T>(this HtmlHelper<T> html, params string[] scripts)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var sb = new StringBuilder();

            foreach(var src in scripts) {
                var tag = new TagBuilder("script");
                
                tag.MergeAttribute("src", src.IsAbsoluteUri() ? src : urlHelper.Relative("~/Scripts/" + src));
                
                sb.AppendLine(tag.ToString());
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString Stylesheets<T>(this HtmlHelper<T> html, params string[] stylesheets)
        {
            var sb = new StringBuilder();

            foreach (var stylesheet in stylesheets)
            {
                sb.Append(html.Stylesheet(stylesheet.ToString()));
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString Stylesheet<T>(this HtmlHelper<T> html, string stylesheet, string rel = "stylesheet")
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var tag = new TagBuilder("link");
            
            tag.MergeAttribute("href", stylesheet.IsAbsoluteUri() ? stylesheet : urlHelper.Relative("~/Content/" + stylesheet));
            tag.MergeAttribute("rel", rel);
            
            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString Link<T>(this HtmlHelper<T> html, string rel, string href)
        {
            if (rel.IsEmpty() || href.IsEmpty())
                return null;

            var tag = new TagBuilder("link");

            tag.MergeAttribute("href", href);
            tag.MergeAttribute("rel", rel);

            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString Partial<T>(this HtmlHelper<T> html, string partialViewName, object model, Formatting singleViewDataEntry)
        {
            var viewDataDictionary = new ViewDataDictionary(model);

            viewDataDictionary.AddSingle(singleViewDataEntry);
            
            return html.PartialOrEmpty(partialViewName, model, viewDataDictionary);
        }

        public static MvcHtmlString PartialOrEmpty<T>(this HtmlHelper<T> html, string partialViewName, object model, ViewDataDictionary viewDataDictionary = null)
        {
            if (model.IsEmpty())
                return null;
            
            return html.Partial(partialViewName, model, viewDataDictionary);
        }

        public static MvcHtmlString ActiveText<T>(this HtmlHelper<T> html, string text, string tagName = "b")
        {
            var tag = new TagBuilder(tagName);
            
            tag.AddCssClass("active");
            tag.SetInnerText(text);           
            
            return new MvcHtmlString(tag.ToString());
        }

        private static string[] BooleanAttributes = new string[] { "required", "hidden", "disabled", "checked" };
        
        public static MvcHtmlString Attr<T>(this HtmlHelper<T> html, IDictionary<string, object> attributes)
        {
            if (attributes == null)
                return null;
            
            var markup = new StringBuilder();

            foreach (var attr in attributes)
            {
                markup.AppendSpace();
                markup.Append(attr.Value.ToAttr(attr.Key));
            }

            return new MvcHtmlString(markup.ToString().RemoveDoubleSpaces());
        }

        public static MvcHtmlString RouteDataAttributes<T>(this HtmlHelper<T> html)
        {
            return html.Attr(html.ViewContext.RouteData.ToDataAttributeDictionary());
        }

        public static MvcHtmlString FormattingAttributes<T>(this HtmlHelper<T> html)
        {
            var webFormatting = html.ViewData.GetSingle<Formatting>();

            if(webFormatting == null)
                return null;

            return html.Attr("class", webFormatting.ClassName);
        }

        public static MvcHtmlString OptionalAttr<T>(this HtmlHelper<T> html, string attributeName, object value, bool condition)
        {
            if (!condition)
                return null;

            if (BooleanAttributes.Contains(attributeName))
                return new MvcHtmlString(attributeName);
            
            return value.ToAttr(attributeName);
        }

        public static MvcHtmlString Attr<T>(this HtmlHelper<T> html, string attributeName, object value)
        {
            return value.ToAttr(attributeName);
        }

        public static MvcHtmlString Attr<T>(this HtmlHelper<T> html, string attributeName, string formatString, object value)
        {
            return value.ToAttr(attributeName, formatString);
        }

        public static MvcHtmlString Content<T>(this HtmlHelper<T> html, string markup)
        {
            if (markup.IsEmpty())
                return null;

            markup = markup.RewriteRelativePaths();

            // require block level container
            if (!markup.Contains("<p") && !markup.Contains("<div"))
            {
                markup = string.Format("<p>{0}</p>", markup);
            }

            return new MvcHtmlString(markup);
        }

        public static string InlineData(string variableName, object graph)
        {
            return string.Format("<script>var {0} = {1};</script>", variableName, graph.ToJson());
        }

        public static MvcHtmlString InlineData<T>(this HtmlHelper<T> html, string variableName, object graph)
        {
            return new MvcHtmlString(InlineData(variableName, graph));
        }

        public static IDictionary<string, object> Attributes<T>(this HtmlHelper<T> html, string className = null, string id = null, string style = null)
        {
            var attr = new Dictionary<string, object>();

            attr.AddNonEmptyValue("class", className);
            attr.AddNonEmptyValue("id", id);
            attr.AddNonEmptyValue("style", style);

            return attr;
        }

        public static MvcHtmlString Visible<T>(this HtmlHelper<T> html, bool condition)
        {
            return html.OptionalAttr("hidden", "hidden", !condition); // add hidden attribute based on condition
        }

        private static string BrowserClassName(HttpBrowserCapabilitiesBase browserCapabilities)
        {
            return string.Format("{0} {0}{1}", browserCapabilities.Browser.ToSlug(), browserCapabilities.MajorVersion);
        }

        public static MvcHtmlString BrowserCapabilitiesAttributes<T>(this HtmlHelper<T> htmlHelper)
        {
            var browserCapabilities = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Browser;
            var sb = new StringBuilder();

            sb.AppendDelimitedValue("no-js");
            sb.AppendDelimitedValue(BrowserClassName(browserCapabilities));

            return sb.ToString().ToAttr("class");
        }
    }
}
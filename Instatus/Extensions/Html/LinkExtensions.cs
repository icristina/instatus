using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus;
using System.Web.Mvc.Html;

namespace Instatus
{
    public static class LinkExtensions
    {
        public static MvcHtmlString Anchor<T>(this HtmlHelper<T> html, string text, string href)
        {
            var tag = new TagBuilder("a");

            tag.MergeAttribute("href", href);
            tag.SetInnerText(text);

            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString ExternalLink<T>(this HtmlHelper<T> html, string text, string href)
        {
            var tag = new TagBuilder("a");

            tag.MergeAttribute("href", href);
            tag.MergeAttribute("rel", "external");
            tag.MergeAttribute("target", "_blank");
            tag.SetInnerText(text);

            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString ActionLinkOrText<T>(this HtmlHelper<T> html, bool condition, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            return html.ActionLinkOrText(condition, linkText, actionName, routeValues, null);
        }

        public static MvcHtmlString ActionLinkOrText<T>(this HtmlHelper<T> html, bool condition, string linkText, string actionName, object routeValues)
        {
            return html.ActionLinkOrText(condition, linkText, actionName, new RouteValueDictionary(routeValues), null);
        }

        public static MvcHtmlString ActionLinkOrText<T>(this HtmlHelper<T> html, bool condition, string linkText, string actionName, RouteValueDictionary routeValues, object htmlAttributes)
        {
            if (condition)
            {
                return html.ActiveText(linkText);
            }
            else
            {
                return html.ActionLink(linkText, actionName, routeValues, new RouteValueDictionary(htmlAttributes));
            }
        }

        public static MvcHtmlString ImageLink<T>(this HtmlHelper<T> html, string alternativeText, string contentPath, string actionName, string controllerName = null, string className = null)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var routeData = html.ViewContext.RouteData;
            var markup = string.Format("<a href=\"{0}\" class=\"{1}\"><img src=\"{2}\" alt=\"{3}\"/></a>",
                            urlHelper.Action(actionName, controllerName),
                            className ?? (controllerName ?? routeData.ControllerName()).ToCamelCase(),
                            actionName.ToCamelCase(),
                            urlHelper.Relative(contentPath),
                            alternativeText);

            return new MvcHtmlString(markup);
        }

        public static MvcHtmlString ExternalImageLink<T>(this HtmlHelper<T> html, string alternativeText, string contentPath, string uri)
        {
            if (uri.IsEmpty())
                return html.Image(contentPath, alternativeText);

            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var markup = string.Format("<a href=\"{0}\" rel=\"external\" target=\"_blank\"><img src=\"{1}\" alt=\"{2}\"/></a>",
                            uri,
                            urlHelper.Relative(contentPath),
                            alternativeText);

            return new MvcHtmlString(markup);
        }

        public static MvcHtmlString PageLink<T>(this HtmlHelper<T> html, string linkText, string slug)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            return html.Anchor(linkText, urlHelper.Page(slug));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus;
using Instatus.Models;
using Instatus.Web;
using System.Web.Mvc.Html;
using Instatus.Entities;

namespace Instatus
{
    public static class PagingExtensions
    {
        public static MvcHtmlString Paging<T>(this HtmlHelper<T> html, IWebView webView = null, string seperator = "", string absolutePath = "", bool unorderedList = true)
        {
            int i = 0;
            int prev = i;

            if (webView == null)
                webView = html.ViewContext.ViewData.Model as IWebView;

            var sb = new StringBuilder();
            var query = webView.Query;

            if (unorderedList)
                sb.Append("<ul class=\"pagination\">");

            if (webView.TotalPageCount > 1)
            {
                while (i < webView.TotalPageCount)
                {
                    var label = (i + 1).ToString();

                    if (!seperator.IsEmpty() && i > 0 && i < webView.TotalPageCount)
                    {
                        if (unorderedList)
                        {
                            sb.AppendFormat("<li>{0}</li>", seperator);
                        }
                        else
                        {
                            sb.AppendSpace();
                            sb.Append(seperator);
                            sb.AppendSpace();
                        }
                    }

                    if (unorderedList && query.PageIndex == i)
                        sb.Append("<li class=\"active\">");
                    else if (unorderedList)
                        sb.Append("<li>");

                    MvcHtmlString link;

                    var nextPage = query.WithPageIndex(i).ToRouteValueDictionary();
                    var parentViewContext = html.ViewContext.ParentActionViewContext;

                    if (query.PageIndex == i && unorderedList)
                    {
                        link = html.ActiveText(label, "a");
                    }
                    else if (query.PageIndex == i)
                    {
                        link = html.ActiveText(label);
                    }
                    else if (!absolutePath.IsEmpty())
                    {
                        link = html.Anchor(label, absolutePath + "?" + nextPage.ToQueryString());
                    }
                    else if (parentViewContext != null)
                    {
                        var routeValues = new RouteValueDictionary(parentViewContext.RouteData.Values)
                                                .AddNonEmptyValues(nextPage);

                        link = html.RouteLink(label, routeValues);
                    }
                    else
                    {
                        link = html.ActionLink(label, "Index", query.WithPageIndex(i).ToRouteValueDictionary());
                    }

                    sb.Append(link);

                    if (unorderedList)
                        sb.Append("</li>");

                    prev = i;
                    i = i.IncrementPager(query.PageIndex, webView.TotalPageCount, query.MaxPageCount);

                    if (prev + 1 != i)
                    {
                        if (unorderedList)
                        {
                            sb.Append("<li class=\"disabled\"><a>&hellip;</a></li>");
                        }
                        else
                        {
                            sb.Append("&hellip;");
                        }
                    }
                }
            }

            if (unorderedList)
                sb.Append("</ul>");

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString LeftArrow<T>(this HtmlHelper<T> html)
        {
            return new MvcHtmlString("&larr;");
        }

        public static MvcHtmlString RightArrow<T>(this HtmlHelper<T> html)
        {
            return new MvcHtmlString("&rarr;");
        }

        public static MvcHtmlString TagCloud<T>(this HtmlHelper<T> html, IWebView webView, ICollection<Tag> tags, string seperator = "/")
        {
            if (tags.IsEmpty())
                return null;

            var sb = new StringBuilder();

            foreach (var tag in tags)
            {
                if (!tags.First().Equals(tag))
                    sb.Append(seperator);

                sb.Append(html.ActionLinkOrText(webView.Tags.SelectedValue.AsString().Equals(tag.Name), tag.Name, "Index", webView.Query.WithTag(tag.Name)));
            }

            return new MvcHtmlString(sb.ToString());
        }
    }
}
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
        public static MvcForm BeginMultipartForm<T>(this HtmlHelper<T> html, string actionName = null, string controllerName = null)
        {
            var routeData = html.ViewContext.RouteData;
            return html.BeginForm(actionName ?? routeData.ActionName(), controllerName ?? routeData.ControllerName(), FormMethod.Post, new { enctype = "multipart/form-data" }); 
        }
        
        public static MvcHtmlString ReturnUrl<T>(this HtmlHelper<T> html, string returnUrl = null)
        {
            var request = html.ViewContext.RequestContext.HttpContext.Request;
            return html.Hidden(HtmlConstants.ReturnUrl, returnUrl ?? request.Params[HtmlConstants.ReturnUrl] ?? request.RawUrl);
        }
        
        public static MvcHtmlString Tag<T>(this HtmlHelper<T> html, string tagName, object value)
        {
            if (value.IsEmpty())
                return null;

            var tag = new TagBuilder(tagName);
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

        public static MvcHtmlString Stylesheet<T>(this HtmlHelper<T> html, string stylesheet, string rel = "stylesheet")
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var tag = new TagBuilder("link");
            tag.MergeAttribute("href", stylesheet.IsAbsoluteUri() ? stylesheet : urlHelper.Relative("~/Content/" + stylesheet));
            tag.MergeAttribute("rel", rel);
            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString Viewport<T>(this HtmlHelper<T> html, string content = "initial-scale=1.0, width=device-width")
        {
            return new MvcHtmlString(HtmlBuilder.Meta("viewport", content));
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
            if(condition) {
                return html.ActiveText(linkText);
            } else {
                return html.ActionLink(linkText, actionName, routeValues, new RouteValueDictionary(htmlAttributes));
            }
        }

        public static MvcHtmlString ImageLink<T>(this HtmlHelper<T> html, string alternativeText, string contentPath, string actionName, string controllerName = null)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var routeData = html.ViewContext.RouteData;
            var markup = string.Format("<a href=\"{0}\" class=\"{1} {2}\"><img src=\"{3}\" alt=\"{4}\"/></a>", 
                            urlHelper.Action(actionName, controllerName),
                            (controllerName ?? routeData.ControllerName()).ToCamelCase(),
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

        public static MvcHtmlString ImageButton<T>(this HtmlHelper<T> html, string alternativeText, string contentPath, string type = "submit")
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var markup = string.Format("<button type=\"{0}\"><img src=\"{1}\" alt=\"{2}\"/></button>",
                            type,
                            urlHelper.Relative(contentPath),
                            alternativeText);

            return new MvcHtmlString(markup);
        }

        public static MvcHtmlString Image<T>(this HtmlHelper<T> html, string contentPath, string text = null)
        {
            if (contentPath.IsEmpty())
                return null;
            
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var tag = new TagBuilder("img");
            var alt = text ?? Path.GetFileNameWithoutExtension(contentPath).ToCapitalized();

            tag.MergeAttribute("src", urlHelper.Relative(contentPath));
            tag.MergeAttribute("alt", alt);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString CommandButton<T>(this HtmlHelper<T> html, string text, string commandName = null)
        {
            commandName = commandName.OrDefault(text.ToSlug());
            
            var tag = new TagBuilder("button");

            tag.MergeAttribute("type", "button");
            tag.MergeAttribute("name", commandName);
            tag.MergeAttribute("class", commandName);
            tag.SetInnerText(text);

            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString SubmitButton<T>(this HtmlHelper<T> html, string text = null)
        {
            var tag = new TagBuilder("button");
            
            tag.MergeAttribute("type", "submit");
            tag.SetInnerText(text ?? WebPhrase.Submit);
            
            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString PageLink<T>(this HtmlHelper<T> html, string linkText, string slug)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            return html.Anchor(linkText, urlHelper.Page(slug));
        }

        public static MvcHtmlString ActiveText<T>(this HtmlHelper<T> html, string text)
        {
            var tag = new TagBuilder("b");

            tag.AddCssClass("current");
            tag.SetInnerText(text);
            
            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString Anchor<T>(this HtmlHelper<T> html, string text, string href)
        {
            var tag = new TagBuilder("a");
            
            tag.MergeAttribute("href", href);
            tag.SetInnerText(text);
            
            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString FileInput<T>(this HtmlHelper<T> html, string name = "file")
        {
            var tag = new TagBuilder("input");
            
            tag.MergeAttribute("type", "file");
            tag.MergeAttribute("name", name);
            
            return new MvcHtmlString(tag.ToString());
        }
        
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

            return new MvcHtmlString(new MarkupTemplateService().Process(null, markup));
        }

        public static MvcForm BeginAuthenticatedForm<T>(this HtmlHelper<T> html, string actionName, string controllerName, string areaName)
        {
            return html.BeginForm(actionName, controllerName, new { area = areaName }, FormMethod.Post, new { data_auth_required = true } );
        }

        public static IDictionary<string, object> RequireAuthentication<T>(this HtmlHelper<T> html)
        {
            return new Dictionary<string, object>() {
                { "data-auth-required", true }
            };
        }

        public static MvcHtmlString InlineData<T>(this HtmlHelper<T> html, string variableName, object graph)
        {
            return new MvcHtmlString(string.Format("<script>var {0} = {1};</script>", variableName, graph.ToJson()));
        }

        public static IDictionary<string, object> ValidationAttributes<T>(this HtmlHelper<T> html)
        {
            var modelMetadata = html.ViewData.ModelMetadata;
            var attr = new Dictionary<string, object>();

            if(modelMetadata.IsRequired)
                attr.Add("required", "required");

            if(!modelMetadata.Watermark.IsEmpty())
                attr.Add("placeholder", modelMetadata.Watermark);

            DataType dataType;

            if (Enum.TryParse<DataType>(modelMetadata.DataTypeName, out dataType) && types.ContainsKey(dataType))
            {
                attr.Add("type", types[dataType]);
            } else if(modelMetadata.Model is Int32) {
                attr.Add("type", "number");
            }

            // http://weblogs.asp.net/rashid/archive/2010/10/21/integrate-html5-form-in-asp-net-mvc.aspx
            foreach(var validator in html.ViewData.ModelMetadata.GetValidators(html.ViewContext)
                                        .SelectMany(v => v.GetClientValidationRules())) {
                var parameters = validator.ValidationParameters;
                
                if(validator is ModelClientValidationRegexRule) {
                    attr.Add("pattern", parameters["pattern"]);
                } else if(validator is ModelClientValidationRangeRule) {
                    attr.Add("min", parameters["min"]);
                    attr.Add("max", parameters["max"]);
                } else if(validator is ModelClientValidationStringLengthRule) {
                    attr.Add("minlength", parameters["minlength"]);
                    attr.Add("maxlength", parameters["maxlength"]);
                }
            }

            return attr;
        }

        private static Dictionary<DataType, string> types = new Dictionary<DataType, string>() {
            { DataType.EmailAddress, "email" },
            { DataType.Text, "text" },
            { DataType.Password, "password" },
            { DataType.PhoneNumber, "tel" },
            { DataType.DateTime, "datetime" },
            { DataType.Date, "date" },
            { DataType.Time, "time" },
            { DataType.Url, "url" }
        };

        public static MvcHtmlString Parts<T>(this HtmlHelper<T> html, WebZone zoneName = WebZone.Body)
        {
            IEnumerable<WebPart> parts;

            if (html.ViewData.Model is Page)
            {
                parts = (html.ViewData.Model as Page).Document.Parts;
            }
            else
            {
                parts = WebPart.Catalog;
            }

            var sb = new StringBuilder();

            foreach (var part in parts.Where(p => p.Zone == zoneName))
            {
                sb.Append(html.Partial("Part", part));
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString Paging<T>(this HtmlHelper<T> html, IWebView webView = null, string seperator = "", string absolutePath = "")
        {
            int i = 0;
            int prev = i;

            if(webView == null)
                webView = html.ViewContext.ViewData.Model as IWebView;

            var sb = new StringBuilder();
            var query = webView.Query;

            if (webView.TotalPageCount > 1)
            {
                while (i < webView.TotalPageCount)
                {
                    var label = (i + 1).ToString();

                    if (!seperator.IsEmpty() && i > 0 && i < webView.TotalPageCount)
                    {
                        sb.AppendSpace();
                        sb.Append(seperator);
                        sb.AppendSpace();
                    }

                    MvcHtmlString link;

                    var nextPage = query.WithPageIndex(i).ToRouteValueDictionary();
                    var parentViewContext = html.ViewContext.ParentActionViewContext;

                    if (query.PageIndex == i)
                    {
                        link = html.ActiveText(label);
                    }
                    else if(!absolutePath.IsEmpty()) {
                        link = html.Anchor(label, absolutePath + "?" + nextPage.ToQueryString());
                    }
                    else if(parentViewContext != null) {
                        var routeValues = new RouteValueDictionary(parentViewContext.RouteData.Values)
                                                .AddNonEmptyValues(nextPage);

                        link = html.RouteLink(label, routeValues);
                    }
                    else
                    {
                        link = html.ActionLink(label, WebAction.Index.ToString(), query.WithPageIndex(i).ToRouteValueDictionary());
                    }

                    sb.Append(link);
                    
                    prev = i;
                    i = i.IncrementPager(query.PageIndex, webView.TotalPageCount, query.MaxPageCount);
                    
                    if (prev + 1 != i)
                    {
                        sb.Append("&hellip;");
                    }
                }
            }

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

        public static MvcHtmlString Visible<T>(this HtmlHelper<T> html, bool condition)
        {
            return condition ? null : new MvcHtmlString("hidden"); // add hidden attribute based on condition
        }

        public static MvcHtmlString Tags<T>(this HtmlHelper<T> html, IWebView webView, ICollection<Tag> tags, string seperator = "/")
        {
            if (tags.IsEmpty())
                return null;
            
            var sb = new StringBuilder();
            
            foreach (var tag in tags)
            {
                if (!tags.First().Equals(tag))
                    sb.Append(seperator);

                sb.Append(html.ActionLinkOrText(webView.Tags.SelectedValue.AsString().Equals(tag.Name), tag.Name, WebAction.Index.ToString(), webView.Query.WithTag(tag.Name)));
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcForm BeginNamedForm<T>(this HtmlHelper<T> html, string id)
        {
            var routeData = html.ViewContext.RouteData;
            return html.BeginForm(routeData.ActionName(), routeData.ControllerName(), FormMethod.Post, new { id = id }); 
        }

        public static MvcHtmlString Title<T>(this HtmlHelper<T> html)
        {
            var title = html.ViewData["Title"].AsString();

            if(title.IsEmpty())
                title = html.ViewData.Model.AsString(); // WebView and Page ToString() customized to give descriptive title for html pages

            return new MvcHtmlString(WebPhrase.HtmlTitle(title));
        }
    }

    internal static class HtmlBuilder
    {
        public static string Embed(string uri)
        {
            var tag = new TagBuilder("iframe");

            tag.MergeAttribute("src", uri);
            tag.MergeAttribute("frameborder", "0");
            tag.MergeAttribute("allowfullscreen", null);

            return tag.ToString();          
        }

        public static string Meta(string name, string content)
        {
            var tag = new TagBuilder("meta");

            tag.MergeAttribute("name", name);
            tag.MergeAttribute("content", content);

            return tag.ToString(TagRenderMode.SelfClosing);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Instatus;
using Instatus.Models;
using Instatus.Web;

namespace Instatus
{
    public static class MetaExtensions
    {
        public static MvcHtmlString Title<T>(this HtmlHelper<T> html)
        {
            var title = html.ViewData["Title"].AsString();
            var model = html.ViewData.Model;

            if (model is IContentItem && (IContentItem)model != null && ((IContentItem)model).Document != null)
                title = ((IContentItem)model).Document.Parameters.Value("html:title", p => p.Content);

            if (title.IsEmpty())
                title = model.AsString(); // WebView and Page ToString() customized to give descriptive title for html pages

            title = WebPhrase.HtmlTitle(title);

            var tagBuilder = new TagBuilder("title");

            tagBuilder.SetInnerText(title);

            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString Meta<T>(this HtmlHelper<T> html, string name, object content)
        {
            if (name.IsEmpty() || content.IsEmpty())
                return null;

            return new MvcHtmlString(HtmlBuilder.Meta(name, content.AsString()));
        }

        public static MvcHtmlString Viewport<T>(this HtmlHelper<T> html, string content = "initial-scale=1.0, width=device-width")
        {
            return new MvcHtmlString(HtmlBuilder.Meta("viewport", content));
        }

        public static MvcHtmlString MetaTags<T>(this HtmlHelper<T> html, IList<Parameter> parameters)
        {
            if (parameters.IsEmpty())
                return null;

            var sb = new StringBuilder();

            foreach (var parameter in parameters)
            {
                sb.AppendLine(HtmlBuilder.Meta(parameter.Name, parameter.Content));
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString MetaTags<T>(this HtmlHelper<T> html)
        {
            var model = html.ViewData.Model;

            if (model is IContentItem && ((IContentItem)model).Document != null)
            {
                var sb = new StringBuilder();
                var parameters = ((IContentItem)model).Document.Parameters;

                sb.AppendLine(HtmlBuilder.Meta("description", parameters.Value("html:description", p => p.Content)));
                sb.AppendLine(HtmlBuilder.Meta("keywords", parameters.Value("html:keywords", p => p.Content)));

                return new MvcHtmlString(sb.ToString());
            }

            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus;
using Instatus.Web;

namespace Instatus
{
    public static class ButtonExtensions
    {
        public static MvcHtmlString ImageButton<T>(this HtmlHelper<T> html, string alternativeText, string contentPath, string type = "submit")
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var markup = string.Format("<button type=\"{0}\"><img src=\"{1}\" alt=\"{2}\"/></button>",
                            type,
                            urlHelper.Relative(contentPath),
                            alternativeText);

            return new MvcHtmlString(markup);
        }

        public static MvcHtmlString CommandButton<T>(this HtmlHelper<T> html, string text, string commandName = null, string className = null)
        {
            commandName = commandName.OrDefault(text.ToSlug());

            var tag = new TagBuilder("button");

            tag.MergeAttribute("type", "button");
            tag.MergeAttribute("name", commandName);
            tag.AddCssClass(commandName);
            tag.AddCssClass(className);
            tag.SetInnerText(text);

            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString SubmitButton<T>(this HtmlHelper<T> html, string text = null, string className = "btn btn-primary")
        {
            var tag = new TagBuilder("button");

            tag.MergeAttribute("type", "submit");
            tag.SetInnerText(text ?? WebPhrase.Submit);
            tag.AddCssClass(className);

            return new MvcHtmlString(tag.ToString());
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus;
using Instatus.Models;
using Instatus.Web;

namespace Instatus
{
    public static class MediaExtensions
    {
        public static MvcHtmlString Image<T>(this HtmlHelper<T> html, string contentPath, string text = null, ImageSize size = ImageSize.Original, string className = null)
        {
            if (contentPath.IsEmpty())
                return null;

            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var tag = new TagBuilder("img");
            var alt = text ?? Path.GetFileNameWithoutExtension(contentPath).ToCapitalized(); // always ensure alt text

            tag.MergeAttribute("src", urlHelper.Resize(size, contentPath));
            tag.MergeAttribute("alt", alt);
            tag.MergeAttributeOrEmpty("class", className);

            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString Thumbnail<T>(this HtmlHelper<T> html, string contentPath, string text = null, string className = "thumbnail")
        {
            return html.Image(contentPath, text, ImageSize.Thumb, className);
        }
    }
}
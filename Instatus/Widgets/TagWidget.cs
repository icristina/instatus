using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus.Models;
using Instatus.Web;
using Instatus;

namespace Instatus.Widgets
{
    public class TagWidget : WebPartial
    {
        private MvcHtmlString mvcHtmlString;
        
        public override object GetViewModel(WebPartialContext context)
        {
            return mvcHtmlString;
        }

        public TagWidget(WebZone zone, string tagName, IDictionary<string, object> attributes, string innerHtml = null, TagRenderMode tagRenderMode = TagRenderMode.Normal, string scope = WebConstant.Scope.Public)
        {
            var tagBuilder = new TagBuilder(tagName);

            tagBuilder.MergeAttributes(attributes);
            tagBuilder.InnerHtml = innerHtml;

            mvcHtmlString = tagBuilder.ToMvcHtmlString(tagRenderMode);

            Zone = zone;
            Scope = scope;
        }

        public static TagWidget Script(string src)
        {
            return new TagWidget(WebZone.Scripts, "script", new Dictionary<string, object>() {
                { "src", src }
            });
        }

        public static TagWidget Stylesheet(string href)
        {
            return new TagWidget(WebZone.Head, "link", new Dictionary<string, object>() {
                { "href", href },
                { "rel", "stylesheet" }
            },
            tagRenderMode: TagRenderMode.SelfClosing);
        }
    }
}
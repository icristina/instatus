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
    public class TagWidget : Part, IModelProvider
    {
        private TagRenderMode tagRenderMode;
        private string innerHtml;
        private string tagName;
        private IDictionary<string, object> attributes;
        
        public object GetModel(ModelProviderContext context)
        {
            var tagBuilder = new TagBuilder(tagName);

            foreach (var attribute in attributes.Where(a => a.Value is string).ToList())
            {
                var virtualPath = attribute.Value as string;

                if (VirtualPathUtility.IsAppRelative(virtualPath))
                {
                    attributes[attribute.Key] = WebPath.Relative(virtualPath);
                }
            }

            tagBuilder.MergeAttributes(attributes);
            tagBuilder.InnerHtml = innerHtml;

            return tagBuilder.ToMvcHtmlString(tagRenderMode);
        }

        public TagWidget(Zone zone, string tagName, IDictionary<string, object> attributes, string innerHtml = null, TagRenderMode tagRenderMode = TagRenderMode.Normal, string scope = WebConstant.Scope.Public)
        {
            this.tagName = tagName;
            this.attributes = attributes;
            this.innerHtml = innerHtml;
            this.tagRenderMode = tagRenderMode;

            Zone = zone;
            Scope = scope;
        }

        public static TagWidget Script(string src, bool footer = true, string scope = WebConstant.Scope.Public)
        {
            var tagWidget = new TagWidget(Zone.Scripts, "script", new Dictionary<string, object>() {
                { "src", src }
            });

            if (!footer)
                tagWidget.Zone = Zone.Head;

            tagWidget.Scope = scope;

            return tagWidget;
        }

        public static TagWidget Stylesheet(string href, string scope = WebConstant.Scope.Public)
        {
            return new TagWidget(Zone.Head, "link", new Dictionary<string, object>() {
                { "href", href },
                { "rel", "stylesheet" }
            },
            tagRenderMode: TagRenderMode.SelfClosing,
            scope: scope);
        }
    }
}
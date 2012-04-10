using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus
{
    public static class TagBuilderExtensions
    {
        public static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode tagRenderMode = TagRenderMode.Normal)
        {
            return new MvcHtmlString(tagBuilder.ToString(tagRenderMode));
        }

        public static TagBuilder MergeDataAttribute(this TagBuilder tagBuilder, string key, object value)
        {
            return tagBuilder.MergeAttributeOrEmpty("data-" + key, value);
        }

        public static TagBuilder MergeAttributeOrEmpty(this TagBuilder tagBuilder, string key, object value)
        {
            if (!(key.IsEmpty() || value.IsEmpty()))
                tagBuilder.MergeAttribute(key, value.AsString());

            return tagBuilder;
        }
    }
}
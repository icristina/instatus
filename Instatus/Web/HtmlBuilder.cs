using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Web
{
    public static class HtmlBuilder
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
            if (name.IsEmpty() || content.IsEmpty())
                return null;
            
            var tag = new TagBuilder("meta");

            tag.MergeAttribute("name", name);
            tag.MergeAttribute("content", content);

            return tag.ToString(TagRenderMode.SelfClosing);
        }

        public static string InlineData(string variableName, object graph)
        {
            return string.Format("<script>var {0} = {1};</script>", variableName, graph.ToJson());
        }
    }
}
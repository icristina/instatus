using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Instatus;

namespace Instatus
{
    public static class InputExtensions
    {
        public static MvcHtmlString FileInput<T>(this HtmlHelper<T> html, string name = "file", string className = null)
        {
            var tag = new TagBuilder("input");

            tag.MergeAttribute("type", "file");
            tag.MergeAttribute("name", name);
            tag.AddCssClass(className);

            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString Options<T>(this HtmlHelper<T> htmlHelper, SelectList selectList, string prefix = null, object value = null)
        {
            var sb = new StringBuilder();

            foreach (SelectListItem item in selectList)
            {
                var option = new TagBuilder("option");

                option.MergeAttribute("value", prefix != null ? string.Format("{0}:{1}", prefix, item.Value) : item.Value);
                option.InnerHtml = item.Text;

                if (item.Selected || (value != null && (item.Value.Equals(value) || item.Text.Match(value)))) // allow passing in value manually, match value exact, match text case insensitive
                    option.MergeAttribute("selected", "selected");

                sb.Append(option.ToString());
            }

            return new MvcHtmlString(sb.ToString());
        }
    }
}
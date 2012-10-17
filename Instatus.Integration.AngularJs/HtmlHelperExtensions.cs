using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.AngularJs
{
    public static class HtmlHelperExtensions
    {
        public static AngularJsTemplateBuilder AngularJsTemplate<T>(this HtmlHelper<T> htmlHelper, string id)
        {
            return new AngularJsTemplateBuilder(htmlHelper, id);
        }

        public class AngularJsTemplateBuilder : IDisposable
        {
            private HtmlHelper htmlHelper;
            private TagBuilder tagBuilder;

            public void Dispose()
            {
                htmlHelper.ViewContext.Writer.WriteLine(tagBuilder.ToString(TagRenderMode.EndTag));
            }

            public AngularJsTemplateBuilder(HtmlHelper htmlHelper, string id)
            {
                this.htmlHelper = htmlHelper;

                tagBuilder = new TagBuilder("script");
                tagBuilder.MergeAttribute("type", "text/ng-template");
                tagBuilder.MergeAttribute("id", id);

                htmlHelper.ViewContext.Writer.WriteLine(tagBuilder.ToString(TagRenderMode.StartTag));
            }
        }
    }
}
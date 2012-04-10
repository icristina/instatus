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
    public class HtmlHelperWidget : WebPartial
    {
        private Func<HtmlHelper<WebPart>, IHtmlString> func;
        
        public override object GetViewModel(WebPartialContext context)
        {
            return func(context.Html);
        }

        public HtmlHelperWidget(Func<HtmlHelper<WebPart>, IHtmlString> func)
        {
            this.func = func;
        }

        public static HtmlHelperWidget Logo()
        {
            return new HtmlHelperWidget(html => html.ImageLink("Logo", "~/Content/logo.png", "Index", "Home", "brand"))
            {
                Zone = WebZone.Banner
            };
        }
    }
}
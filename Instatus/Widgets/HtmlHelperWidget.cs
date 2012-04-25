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
    public class HtmlHelperWidget : Part, IModelProvider
    {
        private Func<HtmlHelper<Part>, IHtmlString> func;
        
        public object GetModel(ModelProviderContext context)
        {
            return func(context.Html);
        }

        public HtmlHelperWidget(Func<HtmlHelper<Part>, IHtmlString> func)
        {
            this.func = func;
        }

        public static HtmlHelperWidget Logo()
        {
            return new HtmlHelperWidget(html => html.ImageLink("Logo", "~/Content/logo.png", "Index", "Home", "brand"))
            {
                Zone = Zone.Banner
            };
        }
    }
}
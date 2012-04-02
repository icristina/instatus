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
    public abstract class BrandingWidget : WebPartial
    {
        public override object GetViewModel(WebPartialContext context)
        {
            return context.Html.ImageLink("Logo", "~/Content/logo.png", "Index", "Home", "brand");
        }
    }
}
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
    public class ThemeWidget : WebPartial
    {
        private string themePath;
        
        public override object GetViewModel(WebPartialContext context)
        {
            return context.Html.Stylesheet(themePath);
        }

        public ThemeWidget(string themePath)
        {
            this.themePath = themePath;

            Zone = WebZone.Head;
            Scope = WebConstant.Scope.Public;
        }
    }
}
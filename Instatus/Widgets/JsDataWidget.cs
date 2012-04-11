using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus.Models;
using Instatus.Web;

namespace Instatus.Widgets
{
    public abstract class JsDataWidget : WebPartial
    {
        private string propertyName;
        
        protected abstract object GetData();
        
        public override object GetViewModel(WebPartialContext context)
        {
            return context.Html.InlineData(propertyName, GetData());
        }

        public JsDataWidget(string propertyName, WebZone zone = WebZone.Scripts)
        {
            this.propertyName = propertyName;

            Zone = zone;
        }
    }
}
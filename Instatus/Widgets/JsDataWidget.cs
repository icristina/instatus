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
    public abstract class JsDataWidget : Part, IModelProvider
    {
        private string propertyName;
        
        protected abstract object GetData();
        
        public object GetModel(ModelProviderContext context)
        {
            return context.Html.InlineData(propertyName, GetData());
        }

        public JsDataWidget(string propertyName, Zone zone = Zone.Scripts)
        {
            this.propertyName = propertyName;

            Zone = zone;
        }
    }
}
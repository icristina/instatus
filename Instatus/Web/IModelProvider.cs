using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus.Models;

namespace Instatus.Web
{   
    public interface IModelProvider
    {
        object GetModel(ModelProviderContext modelProviderContext);
    }

    public class ModelProviderContext
    {
        public UrlHelper Url { get; private set; }
        public HtmlHelper<Part> Html { get; private set; }
        public object ParentModel { get; private set; }
        public RouteData RouteData { get; private set; }

        public ModelProviderContext(UrlHelper urlHelper, HtmlHelper<Part> htmlHelper, object parentModel, RouteData routeData)
        {
            Url = urlHelper;
            Html = htmlHelper;
            ParentModel = parentModel;
            RouteData = routeData;
        }
    }
}
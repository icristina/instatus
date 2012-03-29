using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus.Web;

namespace Instatus.Widgets
{
    public class NavigationWidget : WebPartial
    {
        Func<UrlHelper, SiteMapNodeCollection> buildSiteMapNodeCollection;

        public override object GetModel(RequestContext requestContext)
        {
            var urlHelper = new UrlHelper(requestContext);
            return buildSiteMapNodeCollection(urlHelper);
        }

        public NavigationWidget(Func<UrlHelper, SiteMapNodeCollection> buildSiteMapNodeCollection, WebZone zone = WebZone.Navigation, string viewName = "nav")
        {
            this.buildSiteMapNodeCollection = buildSiteMapNodeCollection;

            Zone = zone;
            ViewName = viewName;
        }
    }
}
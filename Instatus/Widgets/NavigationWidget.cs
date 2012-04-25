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
    public class NavigationWidget : Part, IModelProvider
    {
        Func<UrlHelper, SiteMapNodeCollection> buildSiteMapNodeCollection;

        public object GetModel(ModelProviderContext context)
        {
            var viewDataDictionary = new ViewDataDictionary<SiteMapNodeCollection>(buildSiteMapNodeCollection(context.Url));

            viewDataDictionary.AddSingle(Formatting);
            
            return viewDataDictionary;
        }

        public NavigationWidget(Func<UrlHelper, SiteMapNodeCollection> buildSiteMapNodeCollection, Zone zone = Zone.Navigation, string viewName = "Navigation", Formatting formatting = null, string scope = null)
        {
            this.buildSiteMapNodeCollection = buildSiteMapNodeCollection;

            if (formatting != null)
                Formatting = formatting;

            Zone = zone;
            Template = viewName;
            Scope = scope;
        }

        public NavigationWidget WithClassName(string className)
        {
            Formatting.ClassName = className;
            return this;
        }

        public static NavigationWidget Legal() {
            return new NavigationWidget(url => {
                return url.Navigation()
                    .Home(WebPhrase.Homepage)
                    .Page(WebPhrase.TermsAndConditions, "terms")
                    .Page(WebPhrase.PrivacyPolicy, "privacy")
                    .ToSiteMapNodeCollection();
            }, 
            zone: Zone.Footer,
            formatting: new Formatting() 
            { 
                Label = WebPhrase.Copyright,
                ClassName = "nav-pills"
            },
            scope: "Public");
        }
    }
}
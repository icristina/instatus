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

        public override object GetViewModel(WebPartialContext context)
        {
            var viewDataDictionary = new ViewDataDictionary<SiteMapNodeCollection>(buildSiteMapNodeCollection(context.Url));

            viewDataDictionary.AddSingle(Formatting);
            
            return viewDataDictionary;
        }

        public NavigationWidget(Func<UrlHelper, SiteMapNodeCollection> buildSiteMapNodeCollection, WebZone zone = WebZone.Navigation, string viewName = "Navigation", WebFormatting webFormatting = null, string scope = null)
        {
            this.buildSiteMapNodeCollection = buildSiteMapNodeCollection;

            if (webFormatting != null)
                Formatting = webFormatting;

            Zone = zone;
            ViewName = viewName;
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
            zone: WebZone.Footer,
            webFormatting: new WebFormatting() 
            { 
                Label = WebPhrase.Copyright,
                ClassName = "nav-pills"
            })
            .WithPublicScope();
        }
    }
}
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
        Action<SiteMapNodeCollectionBuilder> action;

        public object GetModel(ModelProviderContext context)
        {
            var builder = context.Url.SiteMapBuilder();

            action(builder);

            var viewDataDictionary = new ViewDataDictionary<SiteMapNodeCollection>(builder.ToSiteMapNodeCollection());

            viewDataDictionary.AddSingle(Formatting);
            
            return viewDataDictionary;
        }

        public NavigationWidget(Action<SiteMapNodeCollectionBuilder> action, Zone zone = Zone.Navigation, string viewName = "Navigation", Formatting formatting = null, string scope = null)
        {
            this.action = action;

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
            return new NavigationWidget(builder => {
                builder
                    .Home(WebPhrase.Homepage)
                    .Page(WebPhrase.TermsAndConditions, "terms")
                    .Page(WebPhrase.PrivacyPolicy, "privacy");
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
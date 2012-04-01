﻿using System;
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
        WebFormatting webFormatting;

        public override object GetModel(WebPartialContext context)
        {
            var viewDataDictionary = new ViewDataDictionary<SiteMapNodeCollection>(buildSiteMapNodeCollection(context.Url));

            viewDataDictionary.AddSingle(webFormatting);
            
            return viewDataDictionary;
        }

        public NavigationWidget(Func<UrlHelper, SiteMapNodeCollection> buildSiteMapNodeCollection, WebZone zone = WebZone.Navigation, string viewName = "nav", WebFormatting webFormatting = null)
        {
            this.buildSiteMapNodeCollection = buildSiteMapNodeCollection;
            this.webFormatting = webFormatting;

            Zone = zone;
            ViewName = viewName;
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
                Label = WebPhrase.Copyright     
            });
        }
    }
}
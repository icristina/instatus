﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus.Models;
using Instatus.Web;

namespace Instatus.Widgets
{
    public abstract class JsApiWidget : WebPartial
    {
        private WebProvider provider;

        public abstract string Embed(UrlHelper urlHelper, Credential credential);
        public abstract object Settings(UrlHelper urlHelper, Credential credential);

        public override object GetModel(RequestContext requestContext)
        {
            return WebCache.Value<MvcHtmlString>(() =>
            {
                UrlHelper urlHelper = new UrlHelper(requestContext);
                Credential credential;

                using (var applicationContext = WebApp.GetService<IApplicationContext>())
                {
                    credential = applicationContext.GetApplicationCredential(provider);
                }

                string inlineData = string.Empty;
                object settings = Settings(urlHelper, credential);

                if (settings != null)
                {
                    inlineData = HtmlHelperExtensions.InlineData(provider.ToString().ToCamelCase() + "Settings", settings);
                }

                string embed = Embed(urlHelper, credential);

                return new MvcHtmlString(inlineData + embed);
            },
            "JspApiWidget." + provider.ToString());
        }

        public JsApiWidget(WebProvider provider, WebZone zone = WebZone.Scripts)
        {
            this.provider = provider;

            Zone = zone;
        }
    }
}
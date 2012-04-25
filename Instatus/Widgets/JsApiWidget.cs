using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus.Entities;
using Instatus.Models;
using Instatus.Web;

namespace Instatus.Widgets
{
    public abstract class JsApiWidget : Part, IModelProvider
    {
        private Provider provider;

        public abstract string Embed(UrlHelper urlHelper, Credential credential);
        public abstract object Settings(UrlHelper urlHelper, Credential credential);

        public object GetModel(ModelProviderContext context)
        {
            return WebCache.Value<IHtmlString>(() =>
            {
                Credential credential = null;

                //using (var applicationContext = WebApp.GetService<IApplicationModel>())
                //{
                //    credential = applicationContext.GetApplicationCredential(provider);
                //}

                if (credential == null)
                    return null;

                string inlineData = string.Empty;
                object settings = Settings(context.Url, credential);

                if (settings != null)
                {
                    inlineData = HtmlHelperExtensions.InlineData(provider.ToString().ToCamelCase() + "Settings", settings);
                }

                string embed = Embed(context.Url, credential);

                return new MvcHtmlString(inlineData + embed);
            },
            "JspApiWidget." + provider.ToString());
        }

        public JsApiWidget(Provider provider, Zone zone = Zone.Scripts)
        {
            this.provider = provider;

            Zone = zone;
        }
    }
}
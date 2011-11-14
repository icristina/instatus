using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Data;
using Instatus.Services;

namespace Instatus.Web
{
    public class PermanentRedirectModule : IHttpModule
    {
        public void Dispose()
        {

        }

        private static List<Link> links;

        private List<Link> Links
        {
            get
            {
                if(links == null) {
                    using (var db = BaseDataContext.Instance().DisableProxiesAndLazyLoading())
                    {
                        links = db.Links.Where(l => l.AlternativeUri != null).ToList();
                    }
                }

                return links;
            }
        }

        public void Init(HttpApplication context)
        {
            PubSub.Provider.Subscribe<ApplicationReset>(a =>
            {
                links = null;
            });            
            
            context.BeginRequest += new EventHandler(this.Alternative);
        }

        private void Alternative(Object source, EventArgs e)
        {
            var link = Links.FirstOrDefault(l => l.Uri == HttpContext.Current.Request.RawUrl);

            if (link != null)
            {
                HttpContext.Current.Response.RedirectPermanent(link.AlternativeUri, true);
            }
        }
    }
}
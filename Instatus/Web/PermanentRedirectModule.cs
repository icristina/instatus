using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Data;

namespace Instatus.Web
{
    public class PermanentRedirectModule : IHttpModule
    {
        public void Dispose()
        {

        }

        private static string canonical;
        private static string[] domains;
        private static List<Link> links;

        public void Init(HttpApplication context)
        {
            using (var db = BaseDataContext.Instance().DisableProxiesAndLazyLoading())
            {
                var canonicalDomain = db.Domains.SingleOrDefault(d => d.IsCanonical);
                
                if(canonicalDomain != null)
                    canonical = canonicalDomain.Uri;

                domains = db.Domains.Where(d => !d.IsCanonical).Select(d => d.Uri).ToArray();
                links = db.Links.Where(l => l.AlternativeUri != null).ToList();
            }

            context.BeginRequest += new EventHandler(this.Canonical);
            context.BeginRequest += new EventHandler(this.Alternative);
        }

        private void Canonical(Object source, EventArgs e)
        {
            if (domains.Any(d => HttpContext.Current.Request.RawUrl.Contains(d)))
            {
                HttpContext.Current.Response.RedirectPermanent(canonical, true);
            }
        }

        private void Alternative(Object source, EventArgs e)
        {
            var link = links.FirstOrDefault(l => l.Uri == HttpContext.Current.Request.RawUrl);

            if (link != null)
            {
                HttpContext.Current.Response.RedirectPermanent(link.AlternativeUri, true);
            }
        }
    }
}
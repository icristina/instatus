using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Data;
using Instatus.Services;

namespace Instatus.Web
{
    public class RedirectModule : IHttpModule
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
                    using (var db = BaseDataContext.BaseInstance())
                    {
                        if (db != null)
                        {
                            links = db.Links.Redirects().ToList();
                        }
                        else
                        {
                            links = new List<Link>();
                        }
                    }
                }

                return links;
            }
        }

        private static List<Domain> domains;

        private List<Domain> Domains
        {
            get
            {
                if (domains == null)
                {
                    using (var db = BaseDataContext.BaseInstance())
                    {
                        if (db != null)
                        {
                            var environment = WebApp.Environment.ToString();
                            var all = WebEnvironment.All.ToString();

                            domains = db.Domains
                                .OrderBy(d => d.IsCanonical)
                                .Where(d => d.Application != null && (d.Environment == environment || d.Environment == all))
                                .ToList();
                        }
                        else
                        {
                            domains = new List<Domain>();
                        }
                    }
                }

                return domains;
            }
        }

        public void Init(HttpApplication context)
        {
            WebApp.OnReset(() =>
            {
                links = null;
                domains = null;
            });

            context.BeginRequest += new EventHandler(this.Alternative);
            context.BeginRequest += new EventHandler(this.Canonical);
        }

        private void Alternative(Object source, EventArgs e)
        {
            var link = Links.FirstOrDefault(l => l.Uri == HttpContext.Current.Request.RawUrl);

            if (link != null)
            {
                if (link.HttpStatusCode == 301)
                {
                    HttpContext.Current.Response.RedirectPermanent(link.Location, true);
                }
                else if(link.HttpStatusCode == 302)
                {
                    HttpContext.Current.Response.Redirect(link.Location, true); // temporary redirect
                }
            }
        }

        private void Canonical(Object source, EventArgs e)
        {
            var url = HttpContext.Current.Request.Url;
            var hostAndPort = url.Authority;
            
            if(Domains.Any(d => d.Uri.Match(hostAndPort) && d.IsCanonical == false) && Domains.Any(d => d.IsCanonical)) {
                var canonicalDomain = Domains.First(d => d.IsCanonical);
                var baseUri = new Uri("http://" + canonicalDomain.Uri);
                var canonicalUri = new Uri(baseUri, url.PathAndQuery);

                HttpContext.Current.Response.RedirectPermanent(canonicalUri.ToString(), true);
            }
        }
    }
}
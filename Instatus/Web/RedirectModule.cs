using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Data;
using Instatus.Services;
using Instatus.Entities;
using System.Web.Mvc;

namespace Instatus.Web
{
    public class RedirectModule : IHttpModule
    {
        public void Dispose()
        {

        }

        private static List<Redirect> redirects;

        private List<Redirect> Redirects
        {
            get
            {
                if(redirects == null) {
                    var applicationModel = DependencyResolver.Current.GetService<IApplicationModel>();

                    if (applicationModel == null)
                    {
                        redirects = new List<Redirect>();
                    }
                    else
                    {
                        redirects = applicationModel.Redirects.ToList();
                    }
                }

                return redirects;
            }
        }

        private static List<Domain> domains;

        private List<Domain> Domains
        {
            get
            {
                if (domains == null)
                {
                    var applicationModel = DependencyResolver.Current.GetService<IApplicationModel>();

                    if (applicationModel == null)
                    {
                        domains = new List<Domain>();
                    }
                    else
                    {
                        var environment = WebApp.Environment.ToString();
                        var all = Deployment.All.ToString();

                        domains = applicationModel.Domains
                            .OrderBy(d => d.Canonical)
                            .Where(d => d.Application != null && (d.Environment == environment || d.Environment == all))
                            .ToList();
                    }
                }

                return domains;
            }
        }

        public void Init(HttpApplication context)
        {
            WebApp.OnReset(() =>
            {
                redirects = null;
                domains = null;
            });

            context.BeginRequest += new EventHandler(this.Alternative);
            context.BeginRequest += new EventHandler(this.Canonical);
        }

        private void Alternative(Object source, EventArgs e)
        {
            var link = Redirects.FirstOrDefault(l => l.Source == HttpContext.Current.Request.RawUrl);

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
            
            if(Domains.Any(d => d.Hostname.Match(hostAndPort) && d.Canonical == false) && Domains.Any(d => d.Canonical)) {
                var canonicalDomain = Domains.First(d => d.Canonical);
                var baseUri = new Uri("http://" + canonicalDomain.Hostname);
                var canonicalUri = new Uri(baseUri, url.PathAndQuery);

                HttpContext.Current.Response.RedirectPermanent(canonicalUri.ToString(), true);
            }
        }
    }
}
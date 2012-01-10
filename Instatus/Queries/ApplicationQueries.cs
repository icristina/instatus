using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Web;
using Instatus.Data;
using System.Data.Entity;

namespace Instatus
{
    public static class ApplicationQueries
    {
        public static Credential GetApplicationCredential(this IBaseDataContext context, WebProvider webProvider, string environment = null)
        {
            if (environment == null)
                environment = HttpContext.Current.ApplicationInstance.Setting<string>("Environment");

            var provider = webProvider.ToString();

            return context.Sources
                    .OfType<Credential>()
                    .FirstOrDefault(s => s.Provider == provider && s.Application != null && s.Environment == environment);
        }

        public static Message GetApplicationMessage(this IBaseDataContext context)
        {
            var published = WebStatus.Published.ToString();
            return context.Messages
                    .Where(m => m.Page is Application && m.Status == published)
                    .OrderByDescending(m => m.CreatedTime)
                    .FirstOrDefault();
        }

        public static IEnumerable<Source> GetApplicationSources(this IBaseDataContext context, WebCategory webCategory)
        {
            var category = webCategory.ToString();

            return context.Sources
                    .Where(s => s.Page is Application && s.Name == category)
                    .ToList();
        }

        public static Application GetCurrentApplication(this IBaseDataContext context)
        {
            return context                    
                    .Pages
                    .Include(a => a.Links)
                    .OfType<Application>()
                    .First();
        }

        public static Brand GetCurrentBrand(this IBaseDataContext context)
        {
            var brand = context.Pages
                    .Include(p => p.Links)
                    .OfType<Brand>()
                    .FirstOrDefault();

            if (brand != null)
                return brand;

            var application = context.GetCurrentApplication();

            return new Brand()
            {
                Name = application.Name,
                Picture = "~/Content/logo.png"
            };
        }

        public static Offer GetLatestOffer(this IBaseDataContext context)
        {
            var now = DateTime.UtcNow;
            var published = WebStatus.Published.ToString();

            return context.Pages.OfType<Offer>()
                    .Include(o => o.Links)
                    .Include(o => o.Dates)
                    .Where(o => o.Dates.Any(d => d.StartTime <= now && (!d.EndTime.HasValue || d.EndTime >= now)) && o.Status == published)
                    .FirstOrDefault();
        }
    }
}
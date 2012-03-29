using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Instatus.Web;

namespace Instatus.Models
{
    [Export(typeof(IPageContext))]
    [PartCreationPolicy(CreationPolicy.NonShared)]     
    public class DbPageContext : IPageContext
    {
        private IApplicationContext applicationContext;
        
        public static string[] DefaultPageExpansions = new string[] { "Restrictions", "Links", "Tags.Taxonomy", "User" };

        public Page GetPage(string slug, WebSet set = null)
        {
            set = set ?? new WebSet();

            var page = applicationContext
                    .Pages
                    .Expand(DefaultPageExpansions)
                    .FilterBySet(set)
                    .Where(p => p.Slug == slug)
                    .FirstOrDefault();

            return ExpandNavigationProperties(page, set);
        }

        private T ExpandNavigationProperties<T>(T page, WebSet set) where T : Page
        {
            if (page != null && applicationContext is DbContext)
            {
                var dbContext = (DbContext)applicationContext;
                
                foreach (var navigationProperty in set.Expand)
                {
                    dbContext.Entry(page).Collection(navigationProperty).Load();
                }
            }

            return page;
        }

        public IEnumerable<Page> GetPages(WebQuery query)
        {
            return applicationContext
                    .Pages
                    .Expand(query.Expand)
                    .FilterBySet(query)
                    .Filter(query)
                    .OfKind(query.Kind)
                    .Sort(query.Sort);
        }
        
        [ImportingConstructor]
        public DbPageContext(IApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext.SerializationSafe();
        }
    }
}
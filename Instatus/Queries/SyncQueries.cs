using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Instatus.Data;
using Instatus.Models;
using System.IO;
using Instatus.Web;

namespace Instatus
{
    public static class SyncQueries
    {
        public static void LoadPages(this BaseDataContext context, Stream stream)
        {
            var pages = Generator.LoadXml<List<Page>>(stream);

            foreach (var page in pages)
            {
                context.AddOrMergePage(page);
                context.SaveChanges();
            }
        }

        public static Page AddOrMergePage(this BaseDataContext context, Page page)
        {
            Page merged;

            if (!page.Sources.IsEmpty())
            {
                var source = page.Sources.First();
                merged = context.Pages.FirstOrDefault(p => p.Sources.Any(s => s.Uri == source.Uri && s.Provider == source.Provider));
            }
            else
            {
                var webSet = new WebSet()
                {
                    Locale = page.Locale
                };                
                merged = context.GetPage(page.Slug, webSet);
            }

            page.Tags = page.Tags.Synchronize(tag => context.Tags.FirstOrDefault(t => t.Name == tag.Name));
            page.Parents = page.Parents.Synchronize(pg => context.Pages.FirstOrDefault(p => p.Slug == pg.Slug), true);

            if (merged == null)
            {
                merged = page;
                merged.Id = 0;
                context.Pages.Add(merged);
            }
            else
            {
                merged.Name = page.Name;
                merged.Description = page.Description;
                merged.Document = page.Document;
                merged.Tags = page.Tags;

                merged.Category = page.Category;

                if (!page.Parents.IsEmpty())
                {
                    context.Entry(merged).Replace(p => p.Parents, page.Parents);
                }

                if (!page.Links.IsEmpty())
                {
                    context.MarkDeleted(merged.Links);
                    merged.Links = page.Links;
                }

                if (page.Priority != 0)
                    merged.Priority = page.Priority;

                if (page is Application)
                {
                    var application = (Application)page;

                    application.Taxonomies = application.Taxonomies.Synchronize(tn => context.Taxonomies.FirstOrDefault(t => t.Name == tn.Name));

                    if (!application.Taxonomies.IsEmpty())
                    {
                        foreach (var taxonomy in application.Taxonomies)
                        {
                            taxonomy.Tags = taxonomy.Tags.Synchronize(tag => context.Tags.FirstOrDefault(t => t.Name == tag.Name));
                        }
                    }

                    context.Entry((Application)merged).Replace(a => a.Taxonomies, application.Taxonomies);
                }
            }

            return merged;
        }
    }

    internal static class DbEntryExtensions
    {
        public static void Replace<T, TNavigation>(this DbEntityEntry<T> entry, Expression<Func<T, ICollection<TNavigation>>> predicate, ICollection<TNavigation> navigation)
            where TNavigation : class
            where T : class
        {
            entry.Collection(predicate).Load();
            entry.Collection(predicate).CurrentValue.Clear();
            entry.Collection(predicate).CurrentValue = navigation;
        }
    }
}
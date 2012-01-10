using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Instatus.Data;
using Instatus.Models;
using System.IO;

namespace Instatus
{
    public static class SyncQueries
    {
        public static void LoadPages(this BaseDataContext context, Stream stream)
        {
            var pages = Generator.LoadXml<List<Page>>(stream);

            foreach (var loaded in pages)
            {
                var page = context.GetPage(loaded.Slug);
                
                loaded.Tags = loaded.Tags.Synchronize(tag => context.Tags.FirstOrDefault(t => t.Name == tag.Name));

                if (page == null)
                {
                    context.Pages.Add(loaded);
                }
                else
                {
                    page.Name = loaded.Name;
                    page.Description = loaded.Description;
                    page.Document = loaded.Document;
                    page.Tags = loaded.Tags;
                    page.Category = loaded.Category;

                    if (!loaded.Links.IsEmpty())
                    {
                        context.MarkDeleted(page.Links);
                        page.Links = loaded.Links;
                    }   

                    if (loaded.Priority != 0)
                        page.Priority = page.Priority;

                    if (loaded is Application)
                    {
                        var application = (Application)loaded;
                        
                        application.Taxonomies = application.Taxonomies.Synchronize(tn => context.Taxonomies.FirstOrDefault(t => t.Name == tn.Name));

                        if (!application.Taxonomies.IsEmpty())
                        {
                            foreach (var taxonomy in application.Taxonomies)
                            {
                                taxonomy.Tags = taxonomy.Tags.Synchronize(tag => context.Tags.FirstOrDefault(t => t.Name == tag.Name));
                            }
                        }

                        context.Entry((Application)page).Replace(a => a.Taxonomies, application.Taxonomies);
                    }
                }

                context.SaveChanges();
            }
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
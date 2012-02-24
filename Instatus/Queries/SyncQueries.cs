﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Instatus.Data;
using Instatus.Models;
using System.IO;
using Instatus.Web;
using System.Data.Entity;

namespace Instatus
{
    public static class SyncQueries
    {
        public static Page GetPage(this BaseDataContext context, Source source)
        {
            return context.Pages.FirstOrDefault(p => p.Sources.Any(s => s.Uri == source.Uri && s.Provider == source.Provider));;
        }        
        
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
                merged = context.GetPage(page.Sources.First());
            }
            else
            {         
                merged = context.Pages.FirstOrDefault(p => p.Slug == page.Slug && p.Locale == page.Locale);
            }

            page.Tags = page.Tags.Synchronize(tag => context.Tags.FirstOrDefault(t => t.Name == tag.Name));
            page.Parents = page.Parents.Synchronize(pg => context.Pages.FirstOrDefault(p => p.Slug == pg.Slug), true);

            if (merged == null)
            {
                merged = page;
                merged.Id = 0; // id should be managed by context
                context.Pages.Add(merged);
            }
            else
            {
                merged.Name = page.Name;
                merged.Description = page.Description;
                merged.Document = page.Document;
                merged.Tags = page.Tags;
                merged.Category = page.Category;
                merged.Picture = page.Picture;

                if (!page.PublishedTime.IsEmpty())
                {
                    merged.PublishedTime = page.PublishedTime;
                }

                if (!page.Parents.IsEmpty())
                {
                    context.Replace(merged, p => p.Parents, page.Parents);
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

                    context.Replace((Application)merged, a => a.Taxonomies, application.Taxonomies);
                }

                if (page is Event)
                {
                    var evnt = (Event)page;
                    var mergedEvent = (Event)merged;

                    context.Entry(mergedEvent).Collection(e => e.Dates).Load();
                    context.MarkDeleted(mergedEvent.Dates);
                    mergedEvent.Dates = evnt.Dates;
                }

                if (page is Place)
                {
                    var place = (Place)page;
                    var mergedPlace = (Place)merged;

                    if(place.Address != null)
                        mergedPlace.Address = place.Address;
                    
                    if(place.Point != null)
                        mergedPlace.Point = place.Point;
                }

                if (!page.Replies.IsEmpty() && page.Replies.OfType<Note>().Any()) // only merge notes, not comments or reviews, which are typically user generated
                {
                    context.MarkDeleted(merged.Replies.OfType<Note>());
                    merged.Replies.Append(page.Replies.OfType<Note>());
                }
            }

            return merged;
        }
    }

    internal static class DbEntryExtensions
    {
        public static void Replace<T, TNavigation>(this DbContext context, T entity, Expression<Func<T, ICollection<TNavigation>>> predicate, ICollection<TNavigation> navigation)
            where TNavigation : class
            where T : class
        {
            context.Entry(entity).Replace(predicate, navigation);
        }
        
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
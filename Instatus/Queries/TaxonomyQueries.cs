using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Data;

namespace Instatus
{
    public static class TaxonomyQueries
    {
        public static IQueryable<Tag> GetTags(this IBaseDataContext context, string taxonomyName)
        {
            return context.Tags.Where(t => t.Taxonomy.Name == taxonomyName).OrderBy(t => t.Name);
        }
    }
}
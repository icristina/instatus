using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using Instatus.Web;

namespace Instatus
{
    public static class ActivityQueries
    {
        public static IEnumerable<Activity> GetActivities(this IApplicationContext context, WebQuery query)
        {
            return context
                    .SerializationSafe()
                    .Activities
                    .Expand(query.Expand)
                    .Filter(query)
                    .Sort(query.Sort);
        }

        public static IEnumerable<T> GetActivities<T>(this IApplicationContext context, WebQuery query) where T : Activity
        {
            query.Kind = typeof(T).Name.AsEnum<WebKind>();

            return context.GetActivities(query).Cast<T>();
        }

        public static IQueryable<Activity> Filter(this IQueryable<Activity> queryable, WebQuery query)
        {
            var filtered = queryable;

            var status = query.Status.ToString();

            filtered = filtered.Where(p => p.Status == status);

            int userId;

            if (!query.User.IsEmpty() && int.TryParse(query.User, out userId))
                filtered = filtered.Where(p => p.UserId == userId);

            int parentId;

            if (!query.Parent.IsEmpty() && int.TryParse(query.Parent, out parentId))
                filtered = filtered.Where(a => a.PageId == parentId);

            if (query.StartDate.HasValue || query.IsDateView)
            {
                DateTime startTime = query.StartDate ?? DateTimeExtensions.StartDate(query.Mode);
                DateTime? endTime = startTime.EndDate(query.Mode);

                query.StartDate = startTime;

                filtered = filtered.Where(p => p.CreatedTime >= startTime);

                if (endTime.HasValue)
                {
                    filtered = filtered.Where(p => p.CreatedTime < endTime.Value);
                }
            }

            return filtered;
        }

        public static IQueryable<Activity> Sort(this IQueryable<Activity> queryable, WebSort sort)
        {
            return queryable.OrderByDescending(a => a.CreatedTime);
        }
    }
}
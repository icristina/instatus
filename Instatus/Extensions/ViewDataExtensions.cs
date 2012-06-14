using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus.Data;
using Instatus.Web;
using Instatus.Models;

namespace Instatus
{
    public static class ViewDataExtensions
    {
        public static void AddGenericError(this ModelStateDictionary modelStateDictionary, string errorMessage)
        {
            modelStateDictionary.AddModelError(Generator.TimeStamp(), errorMessage);
        }        
        
        public static bool HasError(this ModelStateDictionary modelStateDictionary, string propertyName)
        {
            return modelStateDictionary[propertyName] != null && modelStateDictionary[propertyName].Errors != null && modelStateDictionary[propertyName].Errors.Count > 0;
        }
        
        public static bool IsComplete(this ViewDataDictionary viewDataDictionary) {
            return viewDataDictionary.ModelState.IsValid && viewDataDictionary.Model != null;
        }

        // https://github.com/ayende/RaccoonBlog/blob/master/RaccoonBlog.Web/Helpers/ModelStateExtensions.cs
        public static string FirstErrorMessage(this ModelStateDictionary modelStateDictionary)
        {
            return (from modelState in modelStateDictionary
                    from error in modelState.Value.Errors
                    where !string.IsNullOrEmpty(error.ErrorMessage)
                    select error.ErrorMessage).FirstOrDefault(); // first error message
        }

        public static RouteValueDictionary ToRouteValueDictionary(this Query query)
        {
            return new RouteValueDictionary(new Dictionary<string, object>()
                            .AddNonEmptyValue("user", query.User)
                            .AddNonEmptyValue("tag", query.Tag)
                            .AddNonEmptyValue("sortOrder", query.SortOrder)
                            .AddNonEmptyValue("viewMode", query.ViewMode)
                            .AddNonEmptyValue("command", query.Command)
                            .AddNonEmptyValue("contentType", query.ContentType)
                            .AddNonEmptyValue("pageIndex", query.PageIndex)
                            .AddNonEmptyValue("pageSize", query.PageSize)
                            .AddNonEmptyValue("maxPageCount", query.MaxPageCount)
                            .AddNonEmptyValue("countTotal", query.CountTotal)
                            .AddNonEmptyValue("ancestor", query.Ancestor)
                            .AddNonEmptyValue("parent", query.Parent)
                            .AddNonEmptyValue("startDate", query.StartDate.HasValue ? query.StartDate.Value.ToString("yyyy-MM-dd") : null)
                            .AddNonEmptyValue("term", query.Term)
                            .AddNonEmptyValue("filter", query.Filter)
                            .AddNonEmptyValue("category", query.Category)
                            .AddNonEmptyValue("latitude", query.Latitude)
                            .AddNonEmptyValue("longitude", query.Longitude)
                            .AddNonEmptyValue("zoom", query.Zoom)
                            .AddNonEmptyValue("kind", query.Kind)
                            .AddNonEmptyValue("locale", query.Locale)
                            .AddNonEmptyValue("expand", string.Join(",", query.Expand ?? new string[] { })));
        }

        public static IDictionary<string, object> ToDataAttributeDictionary(this Query query)
        {
            return new Dictionary<string, object>()
                .AddNonEmptyValue("data-set-kind", query.Kind.ToString().ToCamelCase())
                .AddNonEmptyValue("data-query-viewMode", query.ViewMode.ToString().ToCamelCase())
                .AddNonEmptyValue("data-query-pageSize", query.PageSize)
                .AddNonEmptyValue("data-query-category", query.Category.ToCamelCase())
                .AddNonEmptyValue("data-query-tag", query.Tag.ToCamelCase());
        }
    }
}
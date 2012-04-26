using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus;
using Instatus.Entities;
using Instatus.Models;

namespace Instatus.Web
{
    public class AddPartsAttribute : ActionFilterAttribute
    {
        public string Scope { get; set; }
        
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewData = filterContext.Controller.ViewData;
            var routeData = filterContext.RouteData;
            var viewModel = viewData.Model;
            var contentItem = viewData.Model as IContentItem ?? new Page();

            if (contentItem.Document == null)
            {
                contentItem.Document = new Document();
            }

            // include parts that are unscoped or scope matches routeData parameter
            var scope = new List<string>();

            if (viewModel != null)
                scope.Add(viewModel.GetType().Name);

            if (routeData != null)
            {
                scope.Add(routeData.ControllerName());
                scope.Add(routeData.ActionName());
                scope.Add(routeData.AreaName());
                scope.Add(routeData.ToUniqueId());
            }

            scope.Add(Scope);

            contentItem.Document.Parts.AddRange(WebCatalog.Parts.Where(p => p.Scope.IsEmpty() || scope.Intersect(p.Scope.ToList(' '), StringComparer.OrdinalIgnoreCase).Any()));

            viewData.AddSingle<IContentItem>(contentItem);

            base.OnActionExecuted(filterContext);
        }
    }
}
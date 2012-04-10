using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus;

namespace Instatus.Web
{
    public class WebPartsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewData = filterContext.Controller.ViewData;
            var routeData = filterContext.RouteData;
            var viewModel = viewData.Model;
            var contentItem = viewData.Model as IContentItem ?? viewData.GetSingle<WebContentItem>();

            if (contentItem == null) {
                contentItem = new WebContentItem();                
            }

            if (contentItem.Document == null)
            {
                contentItem.Document = new WebDocument();
            }

            // include WebParts that are unscoped or scope matches routeData parameter
            var scope = new List<string>();

            scope.Add(viewModel.GetType().Name);

            if (routeData != null)
            {
                scope.Add(routeData.ControllerName());
                scope.Add(routeData.ActionName());
                scope.Add(routeData.AreaName());
                scope.Add(routeData.ToUniqueId());
            }

            var controllerScope = filterContext.Controller.GetCustomAttributeValue<WebDescriptorAttribute, string>(a => a.Scope);

            if (controllerScope.NonEmpty())
            {
                scope.Add(controllerScope);
            }

            contentItem.Document.Parts.AddRange(WebPart.Catalog.Where(p => p.Scope.IsEmpty() || scope.Intersect(p.Scope.ToList(' '), StringComparer.OrdinalIgnoreCase).Any()));

            if(contentItem is WebContentItem)
                viewData.AddSingle(contentItem);

            base.OnActionExecuted(filterContext);
        }
    }
}
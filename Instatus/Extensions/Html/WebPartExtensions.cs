using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus.Web;
using Instatus;
using System.Web.Mvc.Html;

namespace Instatus
{
    public static class WebPartExtensions
    {
        public static MvcHtmlString Parts<T>(this HtmlHelper<T> html, WebZone zoneName = WebZone.Body)
        {
            var viewModel = html.ViewData.Model;
            var routeData = html.ViewContext.RouteData;
            var parts = new List<WebPart>();

            if (viewModel is IContentItem)
            {
                var contentItem = (IContentItem)viewModel;

                if (contentItem.Document != null)
                    parts.AddRange(contentItem.Document.Parts);
            }

            // include WebParts that are unscoped or scope matches routeData parameter
            var scope = new List<string>();

            if (viewModel != null)
            {
                scope.Add(html.ViewData.Model.GetType().Name);
            }

            if (routeData != null)
            {
                scope.Add(routeData.ControllerName());
                scope.Add(routeData.ActionName());
                scope.Add(routeData.AreaName());
                scope.Add(routeData.ToUniqueId());
            }

            var controllerScope = html.ViewContext.Controller.GetCustomAttributeValue<WebDescriptorAttribute, string>(a => a.Scope);

            if (controllerScope.NonEmpty())
            {
                scope.Add(controllerScope);
            }

            parts.AddRange(WebPart.Catalog.Where(p => p.Scope.IsEmpty() || scope.Intersect(p.Scope.ToList(' '), StringComparer.OrdinalIgnoreCase).Any()));

            var sb = new StringBuilder();

            foreach (var part in parts.Where(p => p.Zone == zoneName))
            {
                sb.Append(html.Part(part).ToString());
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString Part<T>(this HtmlHelper<T> html, WebPart part)
        {
            if (part is WebStream)
            {
                var stream = (WebStream)part;

                return html.Action("Index", "Stream", stream.Query.ToRouteValueDictionary()
                    .AddRequestParams()
                    .AddNonEmptyValue("viewName", stream.ViewName)
                    .AddNonEmptyValue("area", "Microsite"));
            }
            else if (part is WebPartial)
            {
                var webPartial = (WebPartial)part;

                if (webPartial.ActionName.IsEmpty())
                {
                    var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
                    var htmlHelper = WebMock.CreateHtmlHelper(part);
                    var routeData = html.ViewContext.RouteData;
                    var parentModel = html.ViewData.Model;

                    var webPartialContext = new WebPartialContext(urlHelper, htmlHelper, parentModel, routeData);
                    var webPartialModel = webPartial.GetViewModel(webPartialContext);

                    if (webPartialModel is MvcHtmlString)
                    {
                        return (MvcHtmlString)webPartialModel;
                    }
                    else if (webPartialModel is ViewDataDictionary)
                    {
                        return html.Partial(webPartial.ViewName, webPartialModel as ViewDataDictionary);
                    }
                    else
                    {
                        return html.Partial(webPartial.ViewName, webPartialModel);
                    }
                }
                else
                {
                    var routeValues = new RouteValueDictionary() {
                        { "viewName", webPartial.ViewName },
                        { "controller", "home" },
                        { "area", "" }    
                    };

                    foreach (var param in webPartial.Parameters)
                    {
                        routeValues.AddNonEmptyValue(param.Name, param.Content);
                    }

                    return html.Action(webPartial.ActionName, routeValues);
                }
            }
            else if (part is WebSection)
            {
                var webSection = (WebSection)part;

                return html.Partial(webSection.ViewName ?? "Section", webSection);
            }

            return null;
        }
    }
}
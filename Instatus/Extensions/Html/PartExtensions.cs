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
using Instatus.Models;
using Instatus.Entities;

namespace Instatus
{
    public static class PartExtensions
    {
        public static bool HasZone<T>(this HtmlHelper<T> html, Zone zone)
        {
            var contentItem = html.ViewData.GetSingle<Page>();

            return contentItem != null && contentItem.Document != null && contentItem.Document.Parts.Any(p => p.Zone == zone);
        }
        
        public static IHtmlString Parts<T>(this HtmlHelper<T> html, Zone zoneName = Zone.Body)
        {
            var sb = new StringBuilder();
            var contentItem = html.ViewData.GetSingle<IContentItem>();

            foreach (var part in contentItem.Document.Parts.Where(p => p.Zone == zoneName))
            {
                sb.Append(html.Part(part).ToString());
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static IHtmlString Part<T>(this HtmlHelper<T> html, Part part)
        {
            if (part.Query != null)
            {
                return html.Action("Index", "Stream", part.Query.ToRouteValueDictionary()
                    .AddRequestParams()
                    .AddNonEmptyValue("viewName", part.Template)
                    .AddNonEmptyValue("area", "Microsite"));
            }
            else if (part.RouteData != null)
            {
                return html.Action(null, part.RouteData);
            }
            else if (part is IModelProvider)
            {
                var modelProvider = (IModelProvider)part;
                var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
                var htmlHelper = Mock.CreateHtmlHelper(part);
                var routeData = html.ViewContext.RouteData;
                var parentModel = html.ViewData.Model;

                var webPartialContext = new ModelProviderContext(urlHelper, htmlHelper, parentModel, routeData);
                var webPartialModel = modelProvider.GetModel(webPartialContext);

                if (webPartialModel is IHtmlString)
                {
                    return (IHtmlString)webPartialModel;
                }
                else if (webPartialModel is ViewDataDictionary)
                {
                    return html.Partial(part.Template, webPartialModel as ViewDataDictionary);
                }
                else
                {
                    return html.Partial(part.Template, webPartialModel);
                }
            }
            else if (part.Template.IsEmpty())
            {
                return html.Raw(part.Body);
            }
            else
            {
                return html.Partial(part.Template, part);
            }
        }
    }
}
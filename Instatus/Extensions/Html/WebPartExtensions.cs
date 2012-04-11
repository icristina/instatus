﻿using System;
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
        public static bool HasZone<T>(this HtmlHelper<T> html, WebZone zone)
        {
            var contentItem = html.ViewData.GetContentItem();

            return contentItem != null && contentItem.Document != null && contentItem.Document.Parts.Any(p => p.Zone == zone);
        }
        
        public static IHtmlString Parts<T>(this HtmlHelper<T> html, WebZone zoneName = WebZone.Body)
        {
            var sb = new StringBuilder();
            var contentItem = html.ViewData.Model as IContentItem ?? html.ViewData.GetSingle<WebContentItem>();

            foreach (var part in contentItem.Document.Parts.Where(p => p.Zone == zoneName))
            {
                sb.Append(html.Part(part).ToString());
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static IHtmlString Part<T>(this HtmlHelper<T> html, WebPart part)
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
            else if (part is WebMarkup)
            {
                return html.Raw(((WebMarkup)part).Body);
            }

            return null;
        }
    }
}
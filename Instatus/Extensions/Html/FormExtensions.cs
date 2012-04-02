using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Instatus;

namespace Instatus
{
    public static class FormExtensions
    {
        public static MvcForm BeginNamedForm<T>(this HtmlHelper<T> html, string id)
        {
            var routeData = html.ViewContext.RouteData;
            return html.BeginForm(routeData.ActionName(), routeData.ControllerName(), FormMethod.Post, new { id = id });
        }

        public static MvcForm BeginMultipartForm<T>(this HtmlHelper<T> html, string actionName = null, string controllerName = null, string className = null)
        {
            var routeData = html.ViewContext.RouteData;
            return html.BeginForm(actionName ?? routeData.ActionName(), controllerName ?? routeData.ControllerName(), FormMethod.Post, new { enctype = "multipart/form-data", @class = className });
        }
    }
}
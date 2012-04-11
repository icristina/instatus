using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Instatus;
using Instatus.Web;

namespace Instatus
{
    public static class FormExtensions
    {
        public static MvcForm BeginActionForm<T>(this HtmlHelper<T> html,
            object routeValues = null,
            string id = null, 
            string actionName = null, 
            string controllerName = null, 
            string className = "form-horizontal", 
            bool multipart = false,
            bool novalidate = false)
        {
            var routeData = html.ViewContext.RouteData;

            var htmlAttributes = new Dictionary<string, object>()
                .AddNonEmptyValue("id", id ?? string.Format("{0}-{1}", routeData.ActionName().ToCamelCase(), routeData.ControllerName().ToCamelCase()))
                .AddNonEmptyValue("class", className);

            if (multipart || html.ViewData.Model.GetCustomAttributeValue<AllowUploadAttribute, bool>(a => a.Allow))
                htmlAttributes.Add("enctype", "multipart/form-data");

            if (novalidate)
                htmlAttributes.Add("novalidate", "novalidate");

            var routeValueDictionary = new RouteValueDictionary(routeValues);

            return html.BeginForm(actionName ?? routeData.ActionName(), controllerName ?? routeData.ControllerName(), routeValueDictionary, FormMethod.Post, htmlAttributes);
        }
    }
}
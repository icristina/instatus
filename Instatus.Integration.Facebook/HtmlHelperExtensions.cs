using Instatus.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Instatus.Integration.Facebook
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString FacebookCanvasLink<T>(this HtmlHelper<T> htmlHelper, string linkText, string actionName, string controllerName = null, object routeValues = null)
        {
            var facebookSettings = htmlHelper.ViewBag.Facebook as FacebookSettings;
            var pathBuilder = new PathBuilder(facebookSettings.CanvasUrl);
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var relativePath = urlHelper.Action(actionName, controllerName, routeValues).TrimStart('/');
            var correctedRelativePath = relativePath.Substring(relativePath.IndexOf('/'));
            var tagBuilder = new TagBuilder("a");

            tagBuilder.MergeAttribute("href", pathBuilder.Path(correctedRelativePath).ToString());
            tagBuilder.MergeAttribute("target", "_top");
            tagBuilder.InnerHtml = linkText;

            return new MvcHtmlString(tagBuilder.ToString());
        }
    }
}

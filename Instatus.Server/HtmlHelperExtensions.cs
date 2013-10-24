using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Server
{
    public static class HtmlHelperExtensions
    {
        public static string ActiveHint(this HtmlHelper htmlHelper, string path)
        {
            return htmlHelper.ActiveHint(HttpContext.Current.Request.Url.AbsolutePath.Contains(path, StringComparison.OrdinalIgnoreCase));
        }

        public static string ActiveHint(this HtmlHelper htmlHelper, bool condition)
        {
            return condition ? "active" : string.Empty;
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static MvcHtmlString ControlGroupHint(this HtmlHelper htmlHelper, string propertyName, bool hasModelStateError = false)
        {
            var errorHint = hasModelStateError || htmlHelper.ViewData.ModelState.HasError(htmlHelper.ViewData.TemplateInfo, propertyName)
                ? "error" : string.Empty;

            var className = string.Format("form-group input-{0} {1}", propertyName.ToLower(), errorHint);

            return new MvcHtmlString(className.Trim());
        }

        public static MvcHtmlString ValidationHint(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewData.ModelMetadata.IsRequired)
            {
                return new MvcHtmlString("required");
            }
            else
            {
                return MvcHtmlString.Empty;
            }
        }

        public static MvcHtmlString TitleHint(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewData.ModelMetadata.IsRequired)
            {
                return new MvcHtmlString(htmlHelper.ViewData.ModelMetadata.GetRequiredErrorMessage(htmlHelper.ViewContext.Controller.ControllerContext));
            }
            else
            {
                return MvcHtmlString.Empty;
            }
        }
    }
}
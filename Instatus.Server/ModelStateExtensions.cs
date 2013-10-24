using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Server
{
    public static class ModelStateExtensions
    {
        public static bool HasError(this ModelStateDictionary modelState, TemplateInfo templateInfo, string propertyName)
        {
            var htmlFieldPrefix = templateInfo.HtmlFieldPrefix;

            if (!string.IsNullOrEmpty(htmlFieldPrefix))
            {
                propertyName = string.Format("{0}.{1}", htmlFieldPrefix, propertyName);
            }

            return modelState.Any(m => m.Key == propertyName)
                && modelState[propertyName].Errors != null
                && modelState[propertyName].Errors.Count > 0;
        }
    }
}
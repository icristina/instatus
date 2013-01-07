using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public static class ModelStateExtensions
    {
        public static bool HasError(this ModelStateDictionary modelState, string propertyName)
        {
            return modelState.Any(m => m.Key == propertyName)
                && modelState[propertyName].Errors != null
                && modelState[propertyName].Errors.Count > 0;
        }
    }
}

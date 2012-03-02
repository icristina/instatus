using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus
{
    public static class ViewDataExtensions
    {
        public static bool HasError(this ModelStateDictionary modelStateDictionary, string propertyName)
        {
            return modelStateDictionary[propertyName] != null && modelStateDictionary[propertyName].Errors != null && modelStateDictionary[propertyName].Errors.Count > 0;
        }
        
        public static bool IsComplete(this ViewDataDictionary viewDataDictionary) {
            return viewDataDictionary.ModelState.IsValid && viewDataDictionary.Model != null;
        }

        // https://github.com/ayende/RaccoonBlog/blob/master/RaccoonBlog.Web/Helpers/ModelStateExtensions.cs
        public static string FirstErrorMessage(this ModelStateDictionary modelStateDictionary)
        {
            return (from modelState in modelStateDictionary
                    from error in modelState.Value.Errors
                    where !string.IsNullOrEmpty(error.ErrorMessage)
                    select error.ErrorMessage).FirstOrDefault(); // first error message
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus
{
    public static class ViewDataExtensions
    {
        public static bool IsComplete(this ViewDataDictionary viewDataDictionary) {
            return viewDataDictionary.ModelState.IsValid && viewDataDictionary.Model != null;
        }

        public static string ErrorMessage(this ModelStateDictionary modelStateDictionary)
        {
            return (from modelState in modelStateDictionary
                    from error in modelState.Value.Errors
                    where !string.IsNullOrEmpty(error.ErrorMessage)
                    select error.ErrorMessage).FirstOrDefault(); // first error message
        }
    }
}
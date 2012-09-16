using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public class GoogleAnalyticsAttribute : ActionFilterAttribute
    {
        public ICredentialStorage CredentialStorage { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewBag = filterContext.Controller.ViewBag;
            var credential = CredentialStorage.GetCredential(WellKnown.Provider.GoogleAnalytics);

            viewBag.GoogleAnalytics = new GoogleAnalyticsConfig
            {
                WebPropertyId = credential.PublicKey
            };
        }

        public class GoogleAnalyticsConfig
        {
            public string WebPropertyId { get; set; }
        }
    }
}

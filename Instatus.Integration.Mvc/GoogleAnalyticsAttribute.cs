using Instatus.Core;
using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public class GoogleAnalyticsAttribute : ActionFilterAttribute
    {
        public IKeyValueStorage<Credential> Credentials { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewBag = filterContext.Controller.ViewBag;
            var credential = Credentials.Get(WellKnown.Provider.GoogleAnalytics);

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

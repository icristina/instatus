using Instatus.Core;
using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.GoogleAnalytics
{
    public class GoogleAnalyticsAttribute : ActionFilterAttribute
    {
        public ILookup<Credential> Credentials { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewBag = filterContext.Controller.ViewBag;
            var credential = Credentials.Get(WellKnown.Provider.GoogleAnalytics);

            viewBag.GoogleAnalytics = new GoogleAnalyticsData
            {
                WebPropertyId = credential.PublicKey
            };
        }

        public class GoogleAnalyticsData
        {
            public string WebPropertyId { get; set; }
        }
    }
}

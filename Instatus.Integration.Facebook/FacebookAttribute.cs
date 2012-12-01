using Instatus.Core;
using Instatus.Core.Models;
using Instatus.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Facebook
{
    public class FacebookAttribute : ActionFilterAttribute
    {
        public IHosting Hosting { get; set; }
        public ILookup<Credential> Credentials { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewBag = filterContext.Controller.ViewBag;
            var facebookConfig = new FacebookConfig(Hosting, Credentials);

            viewBag.Facebook = facebookConfig.GetSettings();
        }
    }
}
using Instatus.Core;
using Instatus.Core.Models;
using Instatus.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public class FacebookAttribute : ActionFilterAttribute
    {
        public IHosting Hosting { get; set; }
        public IKeyValueStorage<Credential> Credentials { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewBag = filterContext.Controller.ViewBag;
            var credential = Credentials.Get(WellKnown.Provider.Facebook);

            viewBag.Facebook = new FacebookConfig
            {
                AppId = credential.PublicKey,
                ChannelUrl = new PathBuilder(Hosting.BaseAddress)
                                .Path("channel.html")
                                .ToProtocolRelativeUri()
            };
        }

        public class FacebookConfig
        {
            public string AppId { get; set; }
            public string ChannelUrl { get; set; }
        }
    }
}
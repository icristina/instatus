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
            var canvasUrl = new PathBuilder("http://apps.facebook.com/").Path(credential.AccountName).ToString();

            viewBag.Facebook = new FacebookConfig
            {
                AppId = credential.PublicKey,
                ChannelUrl = new PathBuilder(Hosting.BaseAddress)
                                .Path("channel.html")
                                .ToString(),
                CanvasUrl = canvasUrl,
                CanvasOauthUrl = new PathBuilder("https://www.facebook.com/dialog/oauth/")
                                .Query("client_id", credential.PublicKey)
                                .Query("scope", string.Join(",", credential.Claims))
                                .Query("redirect_uri", canvasUrl)                                
                                .ToString()
            };
        }

        public class FacebookConfig
        {
            public string AppId { get; set; }
            public string ChannelUrl { get; set; }
            public string CanvasUrl { get; set; }
            public string CanvasOauthUrl { get; set; }
        }
    }
}
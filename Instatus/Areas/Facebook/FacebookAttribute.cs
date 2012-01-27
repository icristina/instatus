using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus;
using Instatus.Models;
using Instatus.Services;
using System.Web.Helpers;
using Instatus.Areas.Microsite;
using Instatus.Web;

namespace Instatus.Areas.Facebook
{
    public class FacebookAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var signedRequest = Facebook.SignedRequest();

            if (signedRequest != null)
            {
                if(signedRequest.oauth_token != null) {
                    Facebook.Authenticated(signedRequest.oauth_token);
                    HttpContext.Current.User.RefreshFromFormsCookie();
                }

                // redirect slug in app data
                if (signedRequest.app_data != null)
                {
                    string appData = signedRequest.app_data;

                    if (!appData.Contains("{"))
                    {
                        HttpContext.Current.Response.Redirect(WebPath.Home, true);
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus;

namespace Instatus.Areas.Facebook
{
    public class FacebookAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var signedRequest = Facebook.SignedRequest();

            if (signedRequest != null && signedRequest.oauth_token != null)
            {
                Facebook.Authenticated(signedRequest.oauth_token);
                HttpContext.Current.User.RefreshFromFormsCookie();
            }
        }
    }
}
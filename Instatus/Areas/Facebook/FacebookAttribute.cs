using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Areas.Facebook
{
    public class FacebookAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var signedRequest = Facebook.SignedRequest();

                if (signedRequest != null && signedRequest.oauth_token)
                {
                    Facebook.Authenticated(signedRequest.oauth_token);
                    HttpContext.Current.Response.Redirect("/");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
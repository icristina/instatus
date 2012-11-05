using Instatus.Integration.Facebook;
using Instatus.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Sample.Controllers
{
    [IframeCookieSupport]
    public class CanvasController : Controller
    {
        public ActionResult Index(FacebookSignedRequest signedRequest)
        {
            if (signedRequest.OauthToken != null)
            {
                FacebookAuthentication.SetAuthCookie(signedRequest.UserId, signedRequest.OauthToken);
            }
            else
            {
                return new FacebookAuthDialogResult();
            }
            
            return Content("User: " + signedRequest.UserId);
        }

        [Authorize]
        public ActionResult SecondPage()
        {
            return Content("User: " + User.Identity.Name + " AccessToken:" + User.GetAccessToken());
        }
    }
}

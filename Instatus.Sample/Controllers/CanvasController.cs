using Instatus.Integration.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Sample.Controllers
{
    public class CanvasController : Controller
    {
        public ActionResult Index(FacebookSignedRequest signedRequest)
        {
            if (signedRequest.OauthToken != null)
            {
                FacebookAuthentication.SetAuthCookie(signedRequest.UserId, signedRequest.OauthToken);
            }
            
            return Content("User: " + signedRequest.UserId);
        }

        public ActionResult SecondPage()
        {
            return Content("User: " + User.Identity.Name);
        }
    }
}

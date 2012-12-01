using Instatus.Core;
using Instatus.Core.Models;
using Instatus.Integration.Facebook;
using Instatus.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Sample.Controllers
{
    [Facebook]
    [IframeCookieSupport]
    public class CanvasController : Controller
    {
        private IHosting hosting;
        private ILookup<Credential> credentials;
        
        public ActionResult Index(FacebookSignedRequest signedRequest)
        {
            if (signedRequest.OauthToken != null)
            {
                FacebookAuthentication.SetAuthCookie(signedRequest.UserId, signedRequest.OauthToken);
            }
            else
            {
                return new FacebookAuthDialogResult(hosting, credentials);
            }
            
            return View();
        }

        [Authorize]
        public ActionResult SecondPage()
        {
            return Content("User: " + User.Identity.Name + " AccessToken: " + User.GetAccessToken());
        }

        public CanvasController(IHosting hosting, ILookup<Credential> credentials)
        {
            this.hosting = hosting;
            this.credentials = credentials;
        }
    }
}

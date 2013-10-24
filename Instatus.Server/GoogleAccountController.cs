using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Security.Claims;

namespace Instatus.Server
{
    [Authorize]
    public abstract class GoogleAccountController<TUser> : Controller where TUser : Microsoft.AspNet.Identity.IUser
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public abstract UserManager<TUser> UserManager { get; set; }

        [AllowAnonymous]
        [HttpGet]
        [Route("login")]
        public ActionResult Login(string returnUrl)
        {
            ViewData.Model = new LoginModel()
            {
                Message = TempData["message"] as string,
                Providers = HttpContext.GetOwinContext().Authentication.GetExternalAuthenticationTypes(),
                ReturnUrl = returnUrl
            };

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public ActionResult Login(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("Callback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        [Route("authenticate")]
        public async Task<ActionResult> Callback(string returnUrl)
        {
            var externalIdentity = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

            if (externalIdentity == null)
            {
                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }

            var emailAddress = externalIdentity.FindFirstValue(ClaimTypes.Email);
            var user = await UserManager.FindByNameAsync(emailAddress);

            if (user != null)
            {
                await SignInAsync(user, false);

                return RedirectToLocal(returnUrl);
            }
            else
            {
                TempData.Add("message", string.Format("The account {0} is not approved.", emailAddress));

                return RedirectToAction("Login", new { ReturnUrl = returnUrl });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("logoff")]
        public ActionResult LogOff(string returnUrl)
        {
            AuthenticationManager.SignOut();

            return RedirectToLocal(returnUrl);
        }

        private async Task SignInAsync(TUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            var authenticationProperties = new AuthenticationProperties()
            {
                IsPersistent = isPersistent
            };

            AuthenticationManager.SignIn(authenticationProperties, identity);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }

            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Instatus.Controllers;
using Instatus.Data;
using Instatus.Web;

namespace Instatus.Areas.Auth.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult LogOn()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult LogOn(string email, string password, string returnUrl)
        {
            if (Membership.ValidateUser(email, password))
            {
                FormsAuthentication.SetAuthCookie(email, true);
                return Redirect(returnUrl.OrDefault(WebPath.Home));
            }

            ModelState.AddModelError("password", WebPhrase.LogOnErrorDescription);

            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Verification(int id, string token)
        {
            using (var db = BaseDataContext.BaseInstance())
            {
                var user = db.Users.Find(id);

                if (user.ValidateVerificationToken(token))
                {
                    db.SaveChanges();

                    ViewData.Model = user;
                }
                else
                {
                    ModelState.AddModelError("Token", WebPhrase.VerificationTokenRejected);
                }
            }

            return View();
        }

        public ActionResult LogOut(string returnUrl)
        {
            FormsAuthentication.SignOut();
            return Redirect(returnUrl.OrDefault(WebPath.Home));
        }
    }
}

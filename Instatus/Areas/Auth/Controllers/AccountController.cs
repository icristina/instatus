using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Instatus.Controllers;
using Instatus.Data;
using Instatus.Web;
using Instatus.Areas.Auth.Models;

namespace Instatus.Areas.Auth.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult LogOn(LogOnViewModel viewModel)
        {
            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult LogOn(LogOnViewModel viewModel)
        {
            if (ModelState.IsValid && Membership.ValidateUser(viewModel.EmailAddress, viewModel.Password))
            {
                FormsAuthentication.SetAuthCookie(viewModel.EmailAddress, true);
                return Redirect(viewModel.ReturnUrl.OrDefault(WebPath.Home));
            }

            ModelState.AddModelError("Password", WebPhrase.LogOnErrorDescription);

            return View(viewModel);
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

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
using Instatus.Models;
using Instatus.Entities;
using Instatus.Services;

namespace Instatus.Areas.Auth.Controllers
{
    public class AccountController : BaseController<IApplicationModel>
    {
        [HttpGet]
        public ActionResult LogOn(string returnUrl)
        {
            ViewData.Model = new LogOnViewModel(returnUrl);
            return View();
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
            var membershipService = DependencyResolver.Current.GetService<IMembershipService>();
            var user = Context.Users.Find(id);

            if (membershipService.ValidateVerificationToken(id, token))
            {
                Context.SaveChanges();

                ViewData.Model = user;
            }
            else
            {
                ModelState.AddModelError("Token", WebPhrase.VerificationTokenRejected);
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

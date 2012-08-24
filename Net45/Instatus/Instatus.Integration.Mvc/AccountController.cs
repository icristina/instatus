using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Instatus.Core;

namespace Instatus.Integration.Mvc
{
    [Authorize]
    public class AccountController : Controller
    {       
        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogOn(string returnUrl)
        {
            ViewData.Model = new LogOnViewModel(returnUrl);
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(LogOnViewModel viewModel)
        {
            if (ModelState.IsValid && Membership.ValidateUser(viewModel.EmailAddress, viewModel.Password))
            {
                FormsAuthentication.SetAuthCookie(viewModel.EmailAddress, true);
                return Redirect(viewModel.ReturnUrl ?? "/");
            }

            return View(viewModel);
        }

        public ActionResult LogOut(string returnUrl)
        {
            FormsAuthentication.SignOut();
            return Redirect(returnUrl ?? "/");
        }
    }
}

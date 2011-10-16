using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Instatus.Controllers;

namespace Instatus.Areas.Auth.Controllers
{
    public class AccountController : BaseController
    {
        public static string DefaultReturnUrl = "/";
        
        public ActionResult LogOn(string email, string password, string returnUrl)
        {
            if (Membership.ValidateUser(email, password))
            {
                FormsAuthentication.SetAuthCookie(email, true);
                return Redirect(returnUrl.OrDefault(DefaultReturnUrl));
            }

            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult LogOut(string returnUrl)
        {
            FormsAuthentication.SignOut();
            return Redirect(returnUrl.OrDefault(DefaultReturnUrl));
        }
    }
}

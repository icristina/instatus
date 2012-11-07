using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Instatus.Core;
using DotNetOpenAuth.AspNet;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using System.Web.SessionState;
using Instatus.Core.Models;

namespace Instatus.Integration.Mvc
{
    [Authorize]
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class AccountController : Controller
    {
        private IMembership membership;
        private IKeyValueStorage<Credential> credentials;
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewData.Model = new LoginModel(returnUrl);
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel viewModel)
        {
            if (ModelState.IsValid && membership.ValidateUser(viewModel.EmailAddress, viewModel.Password))
            {
                FormsAuthentication.SetAuthCookie(viewModel.EmailAddress, CreatePersistentCookie);
                return Redirect(viewModel.ReturnUrl ?? HomeUrl);
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff(string returnUrl)
        {
            FormsAuthentication.SignOut();
            return Redirect(returnUrl ?? HomeUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            var callbackUrl = GenerateCallbackUrl(returnUrl);
            var securityManager = GetSecurityManager(provider);
            
            return new DelegateResult(c => {
                securityManager.RequestAuthentication(callbackUrl);                     
            });
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var callbackUrl = GenerateCallbackUrl(returnUrl);
            var provider = OpenAuthSecurityManager.GetProviderName(ControllerContext.HttpContext);
            var securityManager = GetSecurityManager(provider);
            var authenticationResult = securityManager.VerifyAuthentication(callbackUrl);

            string userName;

            if (authenticationResult.IsSuccessful &&
                membership.ValidateExternalUser(provider, authenticationResult.ProviderUserId, authenticationResult.ExtraData.ToDictionary(k => k.Key, k => k.Value as object), out userName))
            {
                FormsAuthentication.SetAuthCookie(userName, CreatePersistentCookie);                
                return Redirect(returnUrl ?? HomeUrl);
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }

        public static string[] RegistedProviders = new string[] { "Facebook", "Google" }; 

        public virtual IAuthenticationClient GetAuthenticationClient(string provider)
        {
            if (!RegistedProviders.Contains(provider))
                return null;
            
            var credential = credentials.Get(provider);
            
            switch (provider.ToLower())
            {
                case "facebook":
                    return new FacebookClientExtended(credential.PublicKey, credential.PrivateKey, credential.Claims);
                case "microsoft":
                    return new MicrosoftClient(credential.PublicKey, credential.PrivateKey);
                case "twitter":
                    return new TwitterClient(credential.PublicKey, credential.PrivateKey);               
                case "google":
                    return new GoogleOpenIdClient();
                default:
                    return null;
            }
        }

        private OpenAuthSecurityManager GetSecurityManager(string provider)
        {
            var authenticationClient = GetAuthenticationClient(provider);
            return new OpenAuthSecurityManager(ControllerContext.HttpContext, authenticationClient, null);
        }

        private string GenerateCallbackUrl(string returnUrl)
        {
            return Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl });
        }

        public virtual bool CreatePersistentCookie
        {
            get
            {
                return true;
            }
        }

        public virtual string HomeUrl
        {
            get
            {
                return Url.Action("Index", "Home");
            }
        }

        public AccountController(IMembership membership, IKeyValueStorage<Credential> credentials) 
        {
            this.membership = membership;
            this.credentials = credentials;
        }
    }
}

﻿using System;
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
        private ILookup<Credential> credentials;
        
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
            var callbackUrl = GenerateCallbackUrl(provider, returnUrl);
            var securityManager = GetSecurityManager(provider);
            
            return new DelegateResult(c => {
                securityManager.RequestAuthentication(callbackUrl);                     
            });
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string provider, string returnUrl)
        {
            provider = OpenAuthSecurityManager.GetProviderName(ControllerContext.HttpContext) 
                ?? provider 
                ?? RegistedProviders.First(p => Request.UrlReferrer.Host.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0);
            
            var callbackUrl = GenerateCallbackUrl(provider, returnUrl);
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

        public static string[] RegistedProviders = new string[] { "Facebook", "Google", "Twitter", "Microsoft", "Tumblr" }; 

        public virtual IAuthenticationClient GetAuthenticationClient(string provider)
        {
            if (!RegistedProviders.Any(p => p.Equals(provider, StringComparison.OrdinalIgnoreCase)))
                return null;
            
            var credential = credentials.Get(provider);
            
            switch (provider.ToLower())
            {
                case "facebook":
                    return new FacebookOAuthClient(credential.PublicKey, credential.PrivateKey, credential.Claims);
                case "microsoft":
                    return new MicrosoftClient(credential.PublicKey, credential.PrivateKey);
                case "twitter":
                    return new TwitterClient(credential.PublicKey, credential.PrivateKey);               
                case "google":
                    return new GoogleOpenIdClient();
                case "tumblr":
                    return new TumblrOAuthClient(credential.PublicKey, credential.PrivateKey);     
                default:
                    return null;
            }
        }

        private OpenAuthSecurityManager GetSecurityManager(string provider)
        {
            var authenticationClient = GetAuthenticationClient(provider);
            return new OpenAuthSecurityManager(ControllerContext.HttpContext, authenticationClient, new MockOpenAuthDataProvider());
        }

        private string GenerateCallbackUrl(string provider, string returnUrl)
        {
            return Url.Action("ExternalLoginCallback"); // removed provider and returnUrl query parameters as being ignored by external auth providers on callback
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

        public AccountController(IMembership membership, ILookup<Credential> credentials) 
        {
            this.membership = membership;
            this.credentials = credentials;
        }
    }

    internal class MockOpenAuthDataProvider : IOpenAuthDataProvider
    {
        public string GetUserNameFromOpenAuth(string openAuthProvider, string openAuthId)
        {
            return openAuthId;
        }
    }
}

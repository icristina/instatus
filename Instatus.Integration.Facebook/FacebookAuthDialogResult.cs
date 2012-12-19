﻿using Instatus.Core;
using Instatus.Core.Models;
using Instatus.Core.Utils;
using Instatus.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Instatus.Integration.Facebook
{
    public class FacebookAuthDialogResult : ActionResult
    {
        private FacebookSettings facebookSettings;
        
        public override void ExecuteResult(ControllerContext context)
        {
            var canvasOauthUrl = new PathBuilder("https://www.facebook.com/dialog/oauth/")
                                .Query("client_id", facebookSettings.AppId)
                                .Query("scope", string.Join(",", facebookSettings.MinimumClaims))
                                .Query("redirect_uri", facebookSettings.CanvasUrl)
                                .ToString();

            var clientRedirectResult = new ClientRedirectResult(canvasOauthUrl);

            clientRedirectResult.ExecuteResult(context);
        }

        public FacebookAuthDialogResult(IHosting hosting, ILookup<Credential> credentials)
        {
            this.facebookSettings = new FacebookConfig(hosting, credentials).GetSettings();
        }

        public FacebookAuthDialogResult(FacebookSettings facebookSettings)
        {
            this.facebookSettings = facebookSettings;
        }
    }
}

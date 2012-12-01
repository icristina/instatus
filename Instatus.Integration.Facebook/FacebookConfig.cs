using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Instatus.Core;
using Instatus.Core.Models;
using Instatus.Core.Utils;

namespace Instatus.Integration.Facebook
{
    public class FacebookConfig
    {
        private IHosting hosting;
        private ILookup<Credential> credentials;

        public FacebookSettings GetSettings()
        {
            var credential = credentials.Get(WellKnown.Provider.Facebook);

            return new FacebookSettings
            {
                AppId = credential.PublicKey,
                AppSecret = credential.PrivateKey,
                ChannelUrl = new PathBuilder(hosting.BaseAddress)
                                .Path("channel.html")
                                .ToString(),
                CanvasUrl = new PathBuilder("http://apps.facebook.com/")
                                .Path(credential.AccountName)
                                .ToString(),
                MinimumClaims = credential.Claims
            };
        }

        public FacebookConfig(IHosting hosting, ILookup<Credential> credentials)
        {
            this.hosting = hosting;
            this.credentials = credentials;
        }
    }
}

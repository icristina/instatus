using Instatus.Core;
using Instatus.Core.Utils;
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
        public override void ExecuteResult(ControllerContext context)
        {
            var canvasUrl = new PathBuilder("http://apps.facebook.com/").Path(FacebookConfig.Namespace).ToString();
            var canvasOauthUrl = new PathBuilder("https://www.facebook.com/dialog/oauth/")
                                .Query("client_id", FacebookConfig.Credential.PublicKey)
                                .Query("scope", string.Join(",", FacebookConfig.Credential.Claims))
                                .Query("redirect_uri", canvasUrl)
                                .ToString();

            var response = context.RequestContext.HttpContext.Response;

            response.ContentType = WellKnown.ContentType.Html;
            response.Write(string.Format("<script>window.top.location = '{0}';</script>", canvasOauthUrl));
        }
    }
}

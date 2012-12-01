using Instatus.Core;
using Instatus.Core.Models;
using Instatus.Integration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FB = global::Facebook;

namespace Instatus.Integration.Facebook
{
    public class FacebookSignedRequestBinder : IModelBinder
    {
        private FacebookSettings facebookSettings;
        
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var signedRequestString = controllerContext.HttpContext.Request.Unvalidated["signed_request"];

            if (!string.IsNullOrEmpty(signedRequestString))
            {
                var facebookClient = new FB.FacebookClient();

                object signedRequest;

                if (facebookClient.TryParseSignedRequest(facebookSettings.AppSecret, signedRequestString, out signedRequest)) 
                {
                    var jsonNetSerializer = new JsonNetSerializer();
                    var jsonString = jsonNetSerializer.Stringify(signedRequest);

                    return jsonNetSerializer.Parse<FacebookSignedRequest>(jsonString);
                }
            }

            return null;            
        }

        public FacebookSignedRequestBinder(FacebookSettings facebookSettings)
        {
            this.facebookSettings = facebookSettings;
        }
    }
}

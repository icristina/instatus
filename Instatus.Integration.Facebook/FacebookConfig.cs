using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Instatus.Integration.Facebook.FacebookConfig), "RegisterModelBinders")]

namespace Instatus.Integration.Facebook
{
    public static class FacebookConfig
    {
        public static void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(FacebookSignedRequest), new FacebookSignedRequestBinder());
        }
    }
}

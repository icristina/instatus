using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Instatus.Core;
using Instatus.Core.Models;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Instatus.Integration.Facebook.FacebookConfig), "RegisterModelBinders")]

namespace Instatus.Integration.Facebook
{
    public static class FacebookConfig
    {
        public static void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(FacebookSignedRequest), new FacebookSignedRequestBinder());
        }

        public static Credential Credential
        {
            get
            {
                var keyValueStorage = DependencyResolver.Current.GetService<IKeyValueStorage<Credential>>();
                
                return keyValueStorage.Get(WellKnown.Provider.Facebook);
            }
        }

        public static string AppSecret
        {
            get 
            {
                return Credential.PrivateKey;
            }
        }

        public static string Namespace
        {
            get
            {
                return Credential.AccountName;
            }
        }
    }
}

using Instatus.Core;
using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Instatus.Integration.Facebook
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal class FacebookSignedRequestBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            var hosting = DependencyResolver.Current.GetService<IHosting>();
            var credentials = DependencyResolver.Current.GetService<ILookup<Credential>>();
            var facebookConfig = new FacebookConfig(hosting, credentials);
            
            return new FacebookSignedRequestBinder(facebookConfig.GetSettings());
        }
    }
}

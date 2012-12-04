using Instatus.Core;
using Instatus.Core.Models;
using Instatus.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Instatus.Integration.Facebook
{
    public static class UrlHelperExtensions
    {
        public static MvcHtmlString FacebookCanvasUrl(this UrlHelper urlHelper, string virtualPath)
        {
            var hosting = DependencyResolver.Current.GetService<IHosting>();
            var credentials = DependencyResolver.Current.GetService<ILookup<Credential>>();
            var facebookSettings = new FacebookConfig(hosting, credentials).GetSettings();
            var pathBuilder = new PathBuilder(facebookSettings.CanvasUrl);

            return new MvcHtmlString(pathBuilder
                .Path(virtualPath)
                .ToProtocolRelativeUri());
        }
    }
}

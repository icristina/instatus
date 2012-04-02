using System.Web.Mvc;
using Instatus.Web;
using System.Collections.Generic;
using Instatus.Models;
using Instatus.Widgets;

namespace Instatus.Areas.Google
{
    public class GoogleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Google";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            WebPart.Catalog.Add(new GoogleApiWidget());
        }
    }

    public class GoogleApiWidget : JsApiWidget
    {
        public override string Embed(UrlHelper urlHelper, Credential credential)
        {
            return @"<script>
                var _gaq = _gaq || [];
                _gaq.push(['_setAccount', googleSettings.profileId]);
                _gaq.push(['_trackPageview']);

                (function () {
                    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
                    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
                    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
                })();
            </script>";
        }

        public override object Settings(UrlHelper urlHelper, Credential credential)
        {
            return new
            {
                profileId = credential.Uri
            };
        }

        public GoogleApiWidget()
            : base(WebProvider.Google)
        {
            Scope = WebPart.Constants.Public;
        }
    }
}

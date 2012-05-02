using System.Web.Mvc;
using Instatus.Web;
using System.Collections.Generic;
using Instatus.Models;
using Instatus.Widgets;
using Instatus.Entities;
using Autofac;
using System.Web.Routing;

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
            Startup.Parts.Add(new GoogleAnalyticsApiWidget());
        }
    }

    public class GoogleAreaModule : Module
    {
        public GoogleAreaModule()
        {
            RouteTable.Routes.RegisterArea<GoogleAreaRegistration>();
        }
    }

    public class GoogleAnalyticsApiWidget : JsApiWidget
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
                profileId = credential.ApplicationId
            };
        }

        public GoogleAnalyticsApiWidget()
            : base(Provider.GoogleAnalytics)
        {
            Scope = WebConstant.Scope.Public;
        }
    }
}

using System.Web.Mvc;
using Instatus.Web;
using System.Collections.Generic;
using System;
using Instatus.Models;
using Instatus.Widgets;
using Instatus.Entities;

namespace Instatus.Areas.Facebook
{
    public class FacebookAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Facebook";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            GlobalFilters.Filters.Add(new FacebookAttribute());
            
            context.MapRouteLowercase(
                "Facebook_channel",
                "channel.html",
                new { controller = "Facebook", action = "Channel" },
                null,
                new string[] { "Instatus.Areas.Facebook.Controllers" }
            );            
            
            context.MapRouteLowercase(
                "Facebook_default",
                "Facebook/{controller}/{action}/{id}",
                new { controller = "Tab", action = "Index", id = UrlParameter.Optional, area = AreaName },
                null,
                new string[] { "Instatus.Areas.Facebook.Controllers" }
            );

            WebCatalog.Parts.Add(new FacebookApiWidget());
        }
    }

    public class FacebookApiWidget : JsApiWidget
    {
        public override string Embed(UrlHelper urlHelper, Credential credential)
        {
            return @"<div id='fb-root'></div>
                <script src='//connect.facebook.net/en_US/all.js'></script>
                <script>
                    FB.init(facebookSettings.init);
                    FB.Canvas.setAutoGrow();
                </script>";    
        }

        public override object Settings(UrlHelper urlHelper, Credential credential)
        {
            return new
            {
                init = new
                {
                    appId = credential.ApplicationId,
                    status = true,
                    cookie = true,
                    xfbml = credential.HasFeature("xfbml"),
                    channelUrl = WebPath.MatchProtocol(WebPath.Absolute(urlHelper.Action("Channel", "Facebook", new { area = "Facebook" }))),
                    oauth = true
                },
                scope = credential.Scope
            };
        }

        public FacebookApiWidget()
            : base(Provider.Facebook)
        {
            Scope = WebConstant.Scope.Public;
        }
    }
}

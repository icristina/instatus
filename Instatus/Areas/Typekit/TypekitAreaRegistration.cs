using System.Web.Mvc;
using Instatus.Web;
using System.Collections.Generic;
using Instatus.Widgets;

namespace Instatus.Areas.Typekit
{
    public class TypekitAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Typekit";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRouteLowercase(
                "Typekit_default",
                "Typekit/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, area = AreaName },
                null,
                new string[] { "Instatus.Areas.Typekit.Controllers" }
            );

            WebPart.Catalog.Add(new TypekitApiWidget());
        }
    }

    public class TypekitApiWidget : JsApiWidget
    {
        public TypekitApiWidget()
            : base(WebProvider.Typekit)
        {

        }

        public override string Embed
        {
            get {
                return @"<script src='http://use.typekit.com/{uri}.js'></script>
                    <script>try { Typekit.load(); } catch (e) { }</script>";
            }
        }

        public override object Settings(UrlHelper urlHelper, Models.Credential credential)
        {
            return null;
        }
    }
}

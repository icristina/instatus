using System.Web.Mvc;
using Instatus.Web;
using System.Collections.Generic;
using Instatus.Widgets;
using Instatus.Models;

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

        public override string Embed(UrlHelper urlHelper, Credential credential)
        {
            return string.Format(@"<script src='http://use.typekit.com/{0}.js'></script>
                <script>try { Typekit.load(); } catch (e) { }</script>", credential.Uri);
        }

        public override object Settings(UrlHelper urlHelper, Models.Credential credential)
        {
            return null;
        }
    }
}

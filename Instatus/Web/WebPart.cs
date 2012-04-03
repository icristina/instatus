using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Instatus.Web;
using System.Dynamic;

namespace Instatus.Web
{
    [KnownType(typeof(WebSection))]
    [KnownType(typeof(WebPartial))]
    [KnownType(typeof(WebStream))]
    [KnownType(typeof(WebInclude))]
    public class WebPart
    {
        public string ViewName { get; set; }
        public WebZone Zone { get; set; }
        public WebFormatting Formatting { get; set; }
        public string Scope { get; set; } // actionName, areaName, slug or kind

        private List<WebParameter> parameters;

        public List<WebParameter> Parameters
        {
            get
            {
                if (parameters == null)
                    parameters = new List<WebParameter>();

                return parameters;
            }
            set
            {
                parameters = value;
            }
        }

        public WebPart()
        {
            Zone = WebZone.Body;
            Formatting = new WebFormatting();
        }

        private static List<WebPart> catalog = new List<WebPart>();

        public static List<WebPart> Catalog {
            get
            {
                return catalog;
            }
        }
    }

    public static class WebPartExtensions
    {
        public static T WithScope<T>(this T webPart, params string[] scope) where T : WebPart
        {
            webPart.Scope = string.Join(" ", scope);
            return webPart;
        }

        public static T WithPublicScope<T>(this T webPart) where T : WebPart
        {
            return webPart.WithScope(WebConstant.Scope.Public);
        }
    }
}
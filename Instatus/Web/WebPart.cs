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
        }

        private static List<WebPart> catalog = new List<WebPart>();

        public static List<WebPart> Catalog {
            get
            {
                return catalog;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Instatus.Web
{
    public class WebSection : WebPart
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Abstract { get; set; }
        public string Body { get; set; }

        private List<WebLink> links;

        public List<WebLink> Links
        {
            get
            {
                if (links == null)
                    links = new List<WebLink>();

                return links;
            }
            set
            {
                links = value;
            }
        }

        private List<WebPart> parts;

        public List<WebPart> Parts
        {
            get
            {
                if (parts == null)
                    parts = new List<WebPart>();

                return parts;
            }
            set
            {
                parts = value;
            }
        }

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
    }
}
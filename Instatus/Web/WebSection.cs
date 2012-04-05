using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Instatus.Web
{
    public class WebSection : WebMarkup
    {
        public string Heading { get; set; }
        public string Strapline { get; set; }
        public string Abstract { get; set; }

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

        private List<WebElement> elements;

        public List<WebElement> Elements
        {
            get
            {
                if (elements == null)
                    elements = new List<WebElement>();

                return elements;
            }
            set
            {
                elements = value;
            }
        }
    }
}
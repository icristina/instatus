using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using System.Dynamic;
using System.Runtime.Serialization;
using Instatus.Data;

namespace Instatus.Web
{
    public class WebDocument : IHasValue
    {       
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }

        private List<WebLink> links;

        public List<WebLink> Links {
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
        
        public List<WebParameter> Parameters {
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

        public bool HasValue
        {
            get
            {
                return !(Title.IsEmpty() 
                    && Description.IsEmpty() 
                    && Body.IsEmpty() 
                    && links.IsEmpty() 
                    && parts.IsEmpty() 
                    && parameters.IsEmpty());
            }
        }
    }
}
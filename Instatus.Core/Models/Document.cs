using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Document
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }

        private List<Part> parts;

        public List<Part> Parts {
            get
            {
                if (parts == null)
                    parts = new List<Part>();

                return parts;
            }
            set
            {
                parts = value;
            }        
        }

        private List<Parameter> parameters;

        public List<Parameter> Parameters
        {
            get
            {
                if (parameters == null)
                    parameters = new List<Parameter>();

                return parameters;
            }
            set
            {
                parameters = value;
            }
        }

        private List<Link> links;

        public List<Link> Links
        {
            get
            {
                if (links == null)
                    links = new List<Link>();

                return links;
            }
            set
            {
                links = value;
            }
        }
    }
}

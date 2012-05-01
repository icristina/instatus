using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Part
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string Template { get; set; }
        public Zone Zone { get; set; }
        public string Scope { get; set; }
        public object RouteData { get; set; }

        private List<Part> parts;

        public List<Part> Parts
        {
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

        private List<Element> elements;

        public List<Element> Elements
        {
            get
            {
                if (elements == null)
                    elements = new List<Element>();

                return elements;
            }
            set
            {
                elements = value;
            }
        }        
        
        public Query Query { get; set; } // allow query to be null

        private Formatting formatting;

        public Formatting Formatting
        {
            get
            {
                if (formatting == null)
                    formatting = new Formatting();

                return formatting;
            }
            set
            {
                formatting = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Areas.Microsite
{
    public static class SchemaOrgExtensions
    {
        private static Dictionary<Type, string> schemas = new Dictionary<Type, string>()
        {
            { typeof(Article), "WebPage" },
            { typeof(Post), "BlogPost" },
            { typeof(Event), "Event" },
            { typeof(CaseStudy), "NewsArticle" },
            { typeof(Organization), "Organization" },
            { typeof(Place), "Place" },
            { typeof(Product), "Product" },
            { typeof(Address), "Address" },
            { typeof(Job), "JobPosting" }
        };
        
        public static string GetSchemaOrgType(this object graph)
        {
            var type = graph.GetType();
            return schemas.ContainsKey(type) ? schemas[type] : "CreativeWork";
        }

        public static string GetSchemaOrgUri(this object graph)
        {
            return @"http://schema.org/" + GetSchemaOrgType(graph);
        }
    }
}
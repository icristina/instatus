using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus
{
    public static class SchemaOrgExtensions
    {
        private static Dictionary<Type, string> schemas = new Dictionary<Type, string>()
        {
            { typeof(Application), "WebPage" },
            { typeof(Article), "WebPage" },
            { typeof(Post), "BlogPost" },
            { typeof(Event), "Event" },
            { typeof(CaseStudy), "NewsArticle" },
            { typeof(Organization), "Organization" },
            { typeof(Place), "Place" },
            { typeof(Product), "Product" },
            { typeof(Address), "Address" },
            { typeof(Job), "JobPosting" },
            { typeof(User), "Person" },
            { typeof(Profile), "Person" }
        };
        
        private static string GetTypeName(this object graph)
        {
            var type = graph.GetType();
            return schemas.ContainsKey(type) ? schemas[type] : "CreativeWork";
        }

        public static string GetItemType(this object graph)
        {
            return @"http://schema.org/" + GetTypeName(graph);
        }
    }
}
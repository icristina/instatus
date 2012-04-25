using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Web
{
    public class SchemaOrgVocabulary : IWebVocabulary
    {
        //private static Dictionary<Type, string> schemas = new Dictionary<Type, string>()
        //{
        //    { typeof(Application), "WebPage" },
        //    { typeof(Article), "WebPage" },
        //    { typeof(Post), "BlogPost" },
        //    { typeof(Event), "Event" },
        //    { typeof(CaseStudy), "NewsArticle" },
        //    { typeof(Organization), "Organization" },
        //    { typeof(Place), "Place" },
        //    { typeof(Product), "Product" },
        //    { typeof(Address), "Address" },
        //    { typeof(Job), "JobPosting" },
        //    { typeof(User), "Person" },
        //    { typeof(Profile), "Person" }
        //};

        private string GetTypeName(object graph)
        {
            // var type = graph.GetType();
            // return schemas.ContainsKey(type) ? schemas[type] : "CreativeWork";
            return "WebPage";
        }

        public string GetItemType(object graph)
        {
            return @"http://schema.org/" + GetTypeName(graph);
        }
    }
}
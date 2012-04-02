using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;

namespace Instatus.Web
{
    public class WebParameter
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public WebParameter() { }

        public WebParameter(object name, object content)
        {
            Name = name.AsString();
            Content = content.AsString();
        }

        public WebParameter(string ns, object name, object content)
        {
            Name = ns.IsEmpty() ? name.AsString() : GetNamespacedPropertyName(ns, name.AsString());
            Content = content.AsString();
        }

        public static string GetNamespacedPropertyName(string ns, string name)
        {
            return string.Format("{0}:{1}", ns, name);
        }
    }

    public static class WebParameterExtensions
    {
        public static Dictionary<string, object> GetHtmlAttributes(this List<WebParameter> parameters)
        {
            return new Dictionary<string, object>() {
                { "id", parameters.GetParameter(WebNamespace.Html, "id") },
                { "class", parameters.GetParameter(WebNamespace.Html, "class") }    
            };
        }

        public static void SetParameter(this List<WebParameter> parameters, WebNamespace ns, string name, object content)
        {
            parameters.SetParameter(ns.ToDescriptiveString(), name, content);
        }

        public static void SetParameter(this List<WebParameter> parameters, string ns, string name, object content)
        {
            var parameter = parameters.FirstOrDefault(p => p.Name == WebParameter.GetNamespacedPropertyName(ns, name));
            parameters.ReplaceOrAdd(parameter, new WebParameter(ns, name, content));
        }

        public static string GetParameter(this List<WebParameter> parameters, WebNamespace ns, string name)
        {
            return parameters.GetParameter(ns.ToDescriptiveString(), name);
        }

        public static string GetParameter(this List<WebParameter> parameters, string ns, string name)
        {
            return parameters.GetParameter(WebParameter.GetNamespacedPropertyName(ns, name));
        }

        public static string GetParameter(this List<WebParameter> parameters, string name)
        {
            var parameter = parameters.FirstOrDefault(p => p.Name == name);
            return parameter.IsEmpty() ? string.Empty : parameter.Content;
        }

        public static IDictionary<string, object> GetParameterSet(this List<WebParameter> parameters, WebNamespace ns, bool trimNamespace = false)
        {
            return parameters.GetParameterSet(ns.ToDescriptiveString());
        }

        public static IDictionary<string, object> GetParameterSet(this List<WebParameter> parameters, string ns, bool trimNamespace = false)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var parameter in parameters.Where(p => p.Name.StartsWith(ns)))
            {
                var key = trimNamespace ? parameter.Name.SubstringAfter(":") : parameter.Name;
                
                dictionary[key] = parameter.Content;
            }

            return dictionary;
        }
    }
}
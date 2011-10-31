using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebParameter
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public WebParameter() { }

        public WebParameter(object name, object content)
        {
            Name = name.ToString();
            Content = content.ToString();
        }

        public WebParameter(string ns, object name, object content)
        {
            Name = ns.IsEmpty() ? name.ToString() : GetNamespacedPropertyName(ns, name.ToString());
            Content = content.ToString();
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
                { "id", parameters.GetHtmlId() },
                { "class", parameters.GetHtmlClass() }    
            };
        }
        
        public static string GetHtmlId(this List<WebParameter> parameters)
        {
            return parameters.GetNamespacedParameter(WebNamespace.Html, "id");
        }

        public static string GetHtmlClass(this List<WebParameter> parameters)
        {
            return parameters.GetNamespacedParameter(WebNamespace.Html, "class");
        }

        public static void SetNamespacedParameter(this List<WebParameter> parameters, string ns, string name, string content)
        {
            var parameter = parameters.FirstOrDefault(p => p.Name == WebParameter.GetNamespacedPropertyName(ns, name));

            if (parameter == null)
            {
                parameters.Add(new WebParameter(ns, name, content));
            }
            else
            {
                parameter.Content = content;
            }
        }

        public static string GetNamespacedParameter(this List<WebParameter> parameters, WebNamespace ns, string name)
        {
            return parameters.GetNamespacedParameter(ns.ToDescriptiveString(), name);
        }

        public static string GetNamespacedParameter(this List<WebParameter> parameters, string ns, string name)
        {
            var parameter = parameters.FirstOrDefault(p => p.Name == WebParameter.GetNamespacedPropertyName(ns, name));
            return parameter.IsEmpty() ? string.Empty : parameter.Content;
        }
    }
}
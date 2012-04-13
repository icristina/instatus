using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Instatus;
using Instatus.Web;
using Newtonsoft.Json;

namespace Instatus
{
    public static class FormExtensions
    {
        public static JsMvcForm BeginActionForm<T>(this HtmlHelper<T> html,
            object routeValues = null,
            string id = null,
            string actionName = null,
            string controllerName = null,
            string className = "form-horizontal",
            bool multipart = false,
            bool novalidate = false,
            bool javascriptValidation = true)
        {
            var routeData = html.ViewContext.RouteData;

            var clientId = id ?? string.Format("{0}-{1}", routeData.ActionName().ToCamelCase(), routeData.ControllerName().ToCamelCase());

            var htmlAttributes = new Dictionary<string, object>()
                .AddNonEmptyValue("id", clientId)
                .AddNonEmptyValue("class", className);

            if (multipart || html.ViewData.Model.GetCustomAttributeValue<AllowUploadAttribute, bool>(a => a.Allow))
                htmlAttributes.Add("enctype", "multipart/form-data");

            if (novalidate)
                htmlAttributes.Add("novalidate", "novalidate");

            var routeValueDictionary = new RouteValueDictionary(routeValues);

            // http://aspnetwebstack.codeplex.com/SourceControl/changeset/view/730c683da245#src%2fSystem.Web.Mvc%2fHtml%2fFormExtensions.cs
            var action = UrlHelper.GenerateUrl(null, actionName ?? routeData.ActionName(), controllerName ?? routeData.ControllerName(), routeValueDictionary, html.RouteCollection, html.ViewContext.RequestContext, false);

            var tagBuilder = new TagBuilder("form");

            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("action", action);
            tagBuilder.MergeAttribute("method", HtmlHelper.GetFormMethodString(FormMethod.Post), true);

            html.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));

            return new JsMvcForm(html.ViewContext, clientId, javascriptValidation);
        }
    }

    public class JsMvcForm : IDisposable
    {
        private ViewContext viewContext;
        private string clientId;
        private JsFormContext formContext;

        public JsMvcForm(ViewContext viewContext, string clientId, bool enableJavascriptValidation)
        {
            this.viewContext = viewContext;
            this.clientId = clientId;
            this.formContext = new JsFormContext(enableJavascriptValidation);
            this.viewContext.FormContext = this.formContext;
        }
    
        public void Dispose()
        {
 	        viewContext.Writer.WriteLine("</form>");

            if (formContext.EnableJavascriptValidation)
            {
                var patternValidator = @"jQuery.validator.addMethod('pattern', function(value, element, param) {
                        return this.optional(element) || new RegExp(param, 'i').test(value);
                            }, 'Invalid format.');";


                var validateOptions = new
                {
                    rules = formContext.Rules,
                    messages = formContext.Messages
                };

                viewContext.Writer.WriteLine("<script>{0} $('#{1}').validate({2});</script>", patternValidator, clientId, JsonConvert.SerializeObject(validateOptions));
            }
        }
    }

    public class JsFormContext : FormContext
    {
        public bool EnableJavascriptValidation { get; private set; }
        
        public IDictionary<string, object> Rules { get; set; }
        public IDictionary<string, object> Messages { get; set; }

        public JsFormContext(bool enableJavascriptValidation)
        {
            EnableJavascriptValidation = enableJavascriptValidation;
            Rules = new Dictionary<string, object>();
            Messages = new Dictionary<string, object>();
        }
    }
}
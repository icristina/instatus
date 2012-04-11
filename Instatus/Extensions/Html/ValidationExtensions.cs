using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus;
using System.Web.Helpers;
using System.Dynamic;
using Newtonsoft.Json;

namespace Instatus
{
    public static class ValidationExtensions
    {
        public static IDictionary<string, object> ValidationAttributes<T>(this HtmlHelper<T> html)
        {
            var modelMetadata = html.ViewData.ModelMetadata;
            var attr = new Dictionary<string, object>();

            if (modelMetadata.IsRequired)
                attr.Add("required", "required");

            if (!modelMetadata.Watermark.IsEmpty())
                attr.Add("placeholder", modelMetadata.Watermark);

            DataType dataType;

            if (Enum.TryParse<DataType>(modelMetadata.DataTypeName, out dataType) && types.ContainsKey(dataType))
            {
                attr.Add("type", types[dataType]);
            }
            else if (modelMetadata.Model is Int32)
            {
                attr.Add("type", "number");
            }

            // http://weblogs.asp.net/rashid/archive/2010/10/21/integrate-html5-form-in-asp-net-mvc.aspx
            foreach (var validator in html.ViewData.ModelMetadata.GetValidators(html.ViewContext)
                                        .SelectMany(v => v.GetClientValidationRules()))
            {
                var parameters = validator.ValidationParameters;

                if (validator is ModelClientValidationRegexRule)
                {
                    attr.Add("pattern", parameters["pattern"]);
                }
                else if (validator is ModelClientValidationRangeRule)
                {
                    attr.Add("min", parameters["min"]);
                    attr.Add("max", parameters["max"]);
                }
                else if (validator is ModelClientValidationStringLengthRule)
                {
                    attr.Add("minlength", parameters["minlength"]);
                    attr.Add("maxlength", parameters["maxlength"]);
                }
            }

            return attr;
        }

        private static Dictionary<DataType, string> types = new Dictionary<DataType, string>() {
            { DataType.EmailAddress, "email" },
            { DataType.Text, "text" },
            { DataType.Password, "password" },
            { DataType.PhoneNumber, "tel" },
            { DataType.DateTime, "datetime" },
            { DataType.Date, "date" },
            { DataType.Time, "time" },
            { DataType.Url, "url" }
        };

        public static MvcHtmlString ValidationAlert<T>(this HtmlHelper<T> html)
        {
            if (html.ViewData.ModelState.IsValid)
                return null;

            TagBuilder tagBuilder = new TagBuilder("p");

            tagBuilder.AddCssClass("alert");
            tagBuilder.SetInnerText(html.ViewData.ModelState.FirstErrorMessage());

            return tagBuilder.ToMvcHtmlString();
        }

        public static MvcHtmlString ValidationRules<T>(this HtmlHelper<T> html, string clientId = null)
        {
            if (!HttpContext.Current.Request.Browser.Browser.Match("IE"))
                return null;

            var modelMetadata = html.ViewData.ModelMetadata;

            if (clientId.IsEmpty())
                clientId = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            
            dynamic clientRules = new ExpandoObject();

            clientRules.messages = new ExpandoObject();

            foreach (var validator in html.ViewData.ModelMetadata.GetValidators(html.ViewContext)
                                        .SelectMany(v => v.GetClientValidationRules()))
            {
                var parameters = validator.ValidationParameters;

                if (validator is ModelClientValidationRequiredRule)
                {
                    clientRules.required = true;
                    clientRules.messages.required = validator.ErrorMessage;
                }
                else if (validator is ModelClientValidationRegexRule)
                {
                    clientRules.pattern = parameters["pattern"];
                    clientRules.messages.pattern = validator.ErrorMessage;
                }
                else if (validator is ModelClientValidationRangeRule)
                {
                    clientRules.min = parameters["min"];
                    clientRules.max = parameters["max"];
                    clientRules.messages.min = validator.ErrorMessage;
                    clientRules.messages.max = validator.ErrorMessage;
                }
                else if (validator is ModelClientValidationStringLengthRule)
                {
                    clientRules.minlength = parameters["minlength"];
                    clientRules.maxlength = parameters["maxlength"];
                    clientRules.messages.minlength = validator.ErrorMessage;
                    clientRules.messages.minlength = validator.ErrorMessage;
                }
            }

            DataType dataType;

            if (Enum.TryParse<DataType>(modelMetadata.DataTypeName, out dataType))
            {
                switch(dataType) 
                {
                    case DataType.EmailAddress:
                        clientRules.email = true;
                        break;
                }
            }

            if (modelMetadata.ModelType == typeof(int))
            {
                clientRules.number = true;
            }

            return new MvcHtmlString(string.Format("<script>$(function() {{ $('#{0}').rules('add', {1}); }});</script>", clientId, JsonConvert.SerializeObject(clientRules)));
        }

        public static MvcHtmlString ValidateOnSubmit<T>(this HtmlHelper<T> html, string clientId = null)
        {
            if (!HttpContext.Current.Request.Browser.Browser.Match("IE"))
                return null;            
            
            var routeData = html.ViewContext.RouteData;

            if (clientId.IsEmpty())
                clientId = string.Format("{0}-{1}", routeData.ActionName().ToCamelCase(), routeData.ControllerName().ToCamelCase());

            var patternValidator = @"jQuery.validator.addMethod('pattern', function(value, element, param) {
                    return this.optional(element) || new RegExp(param, 'i').test(value);
                        }, 'Invalid format.');";

            return new MvcHtmlString(string.Format("<script>{0} $('#{1}').validate();</script>", patternValidator, clientId));
        }
    }
}
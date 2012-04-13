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
using Instatus.Web;
using System.Web.Mvc.Html;

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
            var formContext = html.ViewContext.FormContext as JsFormContext;

            if (formContext == null || !formContext.EnableJavascriptValidation)
                return null;

            var modelMetadata = html.ViewData.ModelMetadata;

            if (clientId.IsEmpty())
                clientId = html.ViewData.TemplateInfo.HtmlFieldPrefix;
            
            dynamic clientRules = new ExpandoObject();
            dynamic clientMessages = new ExpandoObject();

            foreach (var validator in html.ViewData.ModelMetadata.GetValidators(html.ViewContext)
                                        .SelectMany(v => v.GetClientValidationRules()))
            {
                var parameters = validator.ValidationParameters;
                var errorMessage = validator.ErrorMessage;

                if (validator is JsModelClientValidationRule)
                {
                    var rule = (JsModelClientValidationRule)validator;

                    clientRules[rule.RuleName] = rule.Test;
                    clientMessages[rule.RuleName] = errorMessage;
                }
                else if (validator is ModelClientValidationRequiredRule)
                {
                    clientRules.required = true;
                    clientMessages.required = errorMessage;
                }
                else if (validator is ModelClientValidationRegexRule)
                {
                    clientRules.pattern = parameters["pattern"];
                    clientMessages.pattern = errorMessage;
                }
                else if (validator is ModelClientValidationRangeRule)
                {
                    clientRules.min = parameters["min"];
                    clientRules.max = parameters["max"];
                    clientMessages.min = errorMessage;
                    clientMessages.max = errorMessage;
                }
                else if (validator is ModelClientValidationStringLengthRule)
                {
                    clientRules.minlength = parameters["minlength"];
                    clientRules.maxlength = parameters["maxlength"];
                    clientMessages.minlength = errorMessage;
                    clientMessages.minlength = errorMessage;
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

            formContext.Rules.Add(clientId, clientRules);
            formContext.Messages.Add(clientId, clientMessages);

            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Web
{
    public class JsModelClientValidationRule : ModelClientValidationRule
    {
        public string RuleName { get; set; }
        public object Test { get; set; }

        public JsModelClientValidationRule(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }

    public class JsValidationAttribute : ValidationAttribute, IClientValidatable
    {
        public string RuleName { get; set; }
        public object Test { get; set; }
        
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new JsModelClientValidationRule(FormatErrorMessage(metadata.DisplayName))
            {
                RuleName = RuleName,
                Test = Test
            };
        }
    }
}
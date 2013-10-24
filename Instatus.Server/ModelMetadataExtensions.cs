using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Server
{
    public static class ModelMetadataExtensions
    {
        public static bool IsCheckbox(this ModelMetadata modelMetadata)
        {
            return modelMetadata.ModelType.Equals(typeof(bool));
        }

        public static ModelMetadata GetProperty(this ModelMetadata modelMetadata, string propertyName)
        {
            return modelMetadata.Properties
                .Where(p => p.PropertyName == propertyName)
                .FirstOrDefault();
        }

        public static string GetErrorMessage(this ModelMetadata modelMetadata, ControllerContext controllerContext)
        {
            return modelMetadata
                .GetValidators(controllerContext)
                .SelectMany(v => v.GetClientValidationRules())
                .Select(v => v.ErrorMessage)
                .FirstOrDefault();
        }

        public static IEnumerable<ModelClientValidationRule> GetValidationRules(this ModelMetadata modelMetadata, ControllerContext controllerContext, Type type)
        {
            return modelMetadata
                 .GetValidators(controllerContext)
                 .SelectMany(v => v.GetClientValidationRules())
                 .Where(v => v.GetType() == type);
        }

        public static string GetErrorMessage(this ModelMetadata modelMetadata, ControllerContext controllerContext, Type type)
        {
            return modelMetadata
                .GetValidationRules(controllerContext, type)
                .Select(v => v.ErrorMessage)
                .FirstOrDefault();
        }

        public static string GetRequiredErrorMessage(this ModelMetadata modelMetadata, ControllerContext controllerContext)
        {
            return modelMetadata.GetErrorMessage(controllerContext, typeof(ModelClientValidationRequiredRule));
        }

        public static string GetStringLengthErrorMessage(this ModelMetadata modelMetadata, ControllerContext controllerContext)
        {
            return modelMetadata.GetErrorMessage(controllerContext, typeof(ModelClientValidationStringLengthRule));
        }

        public static string GetRegexErrorMessage(this ModelMetadata modelMetadata, ControllerContext controllerContext)
        {
            return modelMetadata.GetErrorMessage(controllerContext, typeof(ModelClientValidationRegexRule));
        }

        public static string GetRangeErrorMessage(this ModelMetadata modelMetadata, ControllerContext controllerContext)
        {
            return modelMetadata.GetErrorMessage(controllerContext, typeof(ModelClientValidationRangeRule));
        }

        public static IEnumerable<ModelMetadata> GetEditableProperties<T>(this ViewDataDictionary<T> viewData)
        {
            return viewData
                .ModelMetadata
                .Properties
                .Where(prop => prop.ShowForEdit && !viewData.TemplateInfo.Visited(prop));
        }

        public static int GetRangeMinimum(this ModelMetadata modelMetadata, ControllerContext controllerContext)
        {
            return modelMetadata
                .GetValidationRules(controllerContext, typeof(ModelClientValidationRangeRule))
                .Select(v => (int)v.ValidationParameters["min"])
                .FirstOrDefault();
        }

        public static int GetRangeMaximum(this ModelMetadata modelMetadata, ControllerContext controllerContext)
        {
            return modelMetadata
                .GetValidationRules(controllerContext, typeof(ModelClientValidationRangeRule))
                .Select(v => (int)v.ValidationParameters["max"])
                .FirstOrDefault();
        }
    }
}
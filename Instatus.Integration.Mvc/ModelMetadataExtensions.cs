using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
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

        public static string GetErrorMessage(this ModelMetadata modelMetadata, string propertyName, ControllerContext controllerContext)
        {
            var property = modelMetadata.GetProperty(propertyName);

            if (property == null)
            {
                return string.Empty;
            }
            else
            {
                return property
                    .GetValidators(controllerContext)
                    .SelectMany(v => v.GetClientValidationRules())
                    .Select(v => v.ErrorMessage)
                    .FirstOrDefault();
            }
        }

        public static IEnumerable<ModelMetadata> GetEditableProperties<T>(this ViewDataDictionary<T> viewData)
        {
            return viewData
                .ModelMetadata
                .Properties
                .Where(prop => prop.ShowForEdit && !viewData.TemplateInfo.Visited(prop));
        }
    }
}

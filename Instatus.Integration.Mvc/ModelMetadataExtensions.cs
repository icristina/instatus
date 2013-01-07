using System;
using System.Collections.Generic;
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

        public static IEnumerable<ModelMetadata> GetEditableProperties<T>(this ViewDataDictionary<T> viewData)
        {
            return viewData
                .ModelMetadata
                .Properties
                .Where(prop => prop.ShowForEdit && !viewData.TemplateInfo.Visited(prop));
        }
    }
}

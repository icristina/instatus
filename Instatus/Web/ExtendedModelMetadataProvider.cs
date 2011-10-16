using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Web
{
    public class ExtendedModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            metadata.AdditionalValues["IsComplexTypeEntity"] = attributes.OfType<ComplexTypeAttribute>().Any();

            DataType dataType;

            if (Enum.TryParse<DataType>(metadata.DataTypeName, out dataType) && 
                editFormatString.ContainsKey(dataType))
            {
                metadata.EditFormatString = editFormatString[dataType];
            }

            var columnAttributes = attributes.OfType<ColumnAttribute>();

            if (columnAttributes.Any())
            {
                metadata.AdditionalValues["ColumnName"] = columnAttributes.First().Name;
            }

            return metadata;
        }

        private static Dictionary<DataType, string> editFormatString = new Dictionary<DataType, string>()
        {
            { DataType.Date, "{0:yyyy-MM-dd}" }
        };
    }
}
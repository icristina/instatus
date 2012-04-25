using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus;
using System.ComponentModel.DataAnnotations.Schema;

namespace Instatus.Web
{
    public class ExtendedModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            metadata.AdditionalValues["IsComplexTypeEntity"] = attributes.OfType<ComplexTypeAttribute>().Any();
            metadata.AdditionalValues["IsScaffoldColumn"] = attributes.OfType<ScaffoldColumnAttribute>().Any() && attributes.OfType<ScaffoldColumnAttribute>().First().Scaffold;

            DataType dataType;

            if (Enum.TryParse<DataType>(metadata.DataTypeName, out dataType) && editFormatString.ContainsKey(dataType))
            {
                metadata.EditFormatString = editFormatString[dataType];
            }

            attributes.OfType<CategoryAttribute>().ForFirst(c => metadata.AdditionalValues["Category"] = c.Category);
            attributes.OfType<ColumnAttribute>().ForFirst(c => metadata.AdditionalValues["ColumnName"] = c.Name);
            attributes.OfType<DisplayAttribute>().ForFirst(d => metadata.AdditionalValues["GroupName"] = d.GetGroupName());

            return metadata;
        }

        private static Dictionary<DataType, string> editFormatString = new Dictionary<DataType, string>()
        {
            { DataType.Date, "{0:yyyy-MM-dd}" }
        };
    }
}
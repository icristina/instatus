using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class MetaTagsViewModel : BaseViewModel<WebDocument>
    {
        [DisplayName("Title")]
        public string TitleString { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Keywords { get; set; }

        public override void Load(WebDocument model)
        {
            TitleString = model.Parameters.GetParameter(WebNamespace.Html, "Title");
            Description = model.Parameters.GetParameter(WebNamespace.Html, "Description");
            Keywords = model.Parameters.GetParameter(WebNamespace.Html, "Keywords");
        }

        public override void Save(WebDocument model)
        {
            model.Parameters.SetParameter(WebNamespace.Html, "Title", TitleString);
            model.Parameters.SetParameter(WebNamespace.Html, "Description", Description);
            model.Parameters.SetParameter(WebNamespace.Html, "Keywords", Keywords);
        }
    }
}
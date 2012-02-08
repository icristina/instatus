using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using Instatus.Models;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class MetaTagsViewModel<T> : BaseViewModel<T> where T : Page
    {
        [DisplayName("Title")]
        public string TitleString { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Keywords { get; set; }

        public override void Load(T model)
        {
            var document = model.Document;

            if (document != null)
            {
                TitleString = document.Parameters.GetParameter(WebNamespace.Html, "Title");
                Description = document.Parameters.GetParameter(WebNamespace.Html, "Description");
                Keywords = document.Parameters.GetParameter(WebNamespace.Html, "Keywords");
            }
        }

        public override void Save(T model)
        {
            if(model.Document == null)
                model.Document = new WebDocument();
            
            var document = model.Document;
            
            document.Parameters.SetParameter(WebNamespace.Html, "Title", TitleString);
            document.Parameters.SetParameter(WebNamespace.Html, "Description", Description);
            document.Parameters.SetParameter(WebNamespace.Html, "Keywords", Keywords);
        }
    }
}
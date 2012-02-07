using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using System.Web.Mvc;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class DocumentViewModel : IViewModel<WebDocument>
    {
        public string Heading { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Body { get; set; }

        public void Load(WebDocument model)
        {
            this.ApplyValues(model);
        }

        public void Save(WebDocument model)
        {
            model.ApplyValues(this);
        }

        public void Databind()
        {
            
        }

        [ScaffoldColumn(false)]
        public WebStep Step
        {
            get {
                return WebStep.Start;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using System.Web.Mvc;
using Instatus.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class DocumentViewModel : BaseViewModel<Page>
    {       
        public string Heading { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Body { get; set; }

        public override void Load(Page model)
        {
            Heading = model.Document.Title.TrimOrNull();
            Body = model.Document.Body.TrimOrNull();
        }

        public override void Save(Page model)
        {
            model.Document.Title = Heading.TrimOrNull();
            model.Document.Body = Body.TrimOrNull();
        }
    }
}
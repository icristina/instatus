using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using System.Web.Mvc;
using Instatus.Models;

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
            Heading = model.Document.Title;
            Body = model.Document.Body;
        }

        public override void Save(Page model)
        {
            model.Document.Title = Heading;
            model.Document.Body = Body;
        }
    }
}
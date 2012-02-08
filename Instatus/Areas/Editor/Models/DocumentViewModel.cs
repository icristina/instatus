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
    public class DocumentViewModel<T> : BaseViewModel<T> where T : Page
    {       
        public string Heading { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Body { get; set; }

        public override void Load(T model)
        {
            Heading = model.Document.Title.TrimOrNull();
            Body = model.Document.Body.TrimOrNull();
        }

        public override void Save(T model)
        {
            model.Document.Title = Heading.TrimOrNull();
            model.Document.Body = Body.TrimOrNull();
        }
    }
}
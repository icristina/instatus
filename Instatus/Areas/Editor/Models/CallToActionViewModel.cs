using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using System.Web.Mvc;
using Instatus.Models;
using Instatus.Data;
using Instatus.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class CallToActionViewModel : BaseViewModel<Page>
    {       
        public string Uri { get; set; }

        public override void Load(Page model)
        {
            Uri = model.Document.Links.Uri();
        }

        public override void Save(Page model)
        {
            var html = WebContentType.Html.ToMimeType();
            
            model.Document.Links.Where(l => l.ContentType == html).ToList().ForEach(l => model.Document.Links.Remove(l));

            if (!Uri.IsEmpty())
            {
                model.Document.Links.Add(new Link()
                {
                    ContentType = html,
                    Uri = Uri
                });
            }
        }
    }
}
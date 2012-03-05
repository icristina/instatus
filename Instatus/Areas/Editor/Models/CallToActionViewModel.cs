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

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class CallToActionViewModel<T> : BaseViewModel<T, IApplicationContext> where T : Page
    {       
        public string Uri { get; set; }

        public override void Load(T model)
        {
            Uri = model.Links.Uri();
        }

        public override void Save(T model)
        {
            var html = WebContentType.Html.ToMimeType();
            
            model.Links.Where(l => l.ContentType == html).ToList().ForEach(l => Context.Links.Remove(l));

            if (!Uri.IsEmpty())
            {
                model.Links.Add(new Link()
                {
                    ContentType = html,
                    Uri = Uri
                });
            }
        }
    }
}
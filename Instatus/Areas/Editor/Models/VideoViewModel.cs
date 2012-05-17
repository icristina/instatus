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
using System.ComponentModel.DataAnnotations.Schema;
using Instatus.Entities;

namespace Instatus.Areas.Editor.Models
{
    public class VideoViewModel : BaseViewModel<Page>
    {       
        public string Uri { get; set; }

        public override void Load(Page model)
        {
            Uri = model.Document.Links.Uri(null, WebConstant.Rel.Video);
        }

        public override void Save(Page model)
        {
            model.Document.Links.Where(l => l.Rel.Match(WebConstant.Rel.Video)).ForFirst(v => model.Document.Links.Remove(v));
            
            if (!Uri.IsEmpty())
            {
                model.Document.Links.Add(new Link()
                {
                    Uri = Uri,
                    Rel = WebConstant.Rel.Video
                });
            }            
        }
    }
}
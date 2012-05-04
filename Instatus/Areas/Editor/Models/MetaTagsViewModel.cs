using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using Instatus.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Instatus.Entities;

namespace Instatus.Areas.Editor.Models
{
    public class MetaTagsViewModel : BaseViewModel<Page>
    {
        [DisplayName("Title")]
        public string TitleString { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Keywords { get; set; }

        public override void Load(Page model)
        {
            var document = model.Document;

            if (document != null)
            {
                TitleString = document.Parameters.Value("html:Title", s => s.Content);
                Description = document.Parameters.Value("html:Description", s => s.Content);
                Keywords = document.Parameters.Value("html:Keywords", s => s.Content);
            }
        }

        public override void Save(Page model)
        {
            if(model.Document == null)
                model.Document = new Document();
            
            var document = model.Document;
            
            document.Parameters.Get("html:Title").Content = TitleString;
            document.Parameters.Get("html:Description").Content = Description;
            document.Parameters.Get("html:Keywords").Content = Keywords;
        }
    }
}
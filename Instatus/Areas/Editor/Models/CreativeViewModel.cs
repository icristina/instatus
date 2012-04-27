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
    public class CreativeViewModel : BaseViewModel<Page>
    {
        [ScaffoldColumn(true)]
        [Display(Order = 5)]
        public IEnumerable<LinkViewModel> Links { get; set; }

        public override void Load(Page model)
        {
            base.Load(model);

            Links = model.Document.Links.Select(l => new LinkViewModel(l.Title, l.Uri, l.Picture)).ToList().Pad(10);
        }

        public override void Save(Page model)
        {
            base.Save(model);

            model.Document.Links = Links.RemoveNullOrEmpty().Select(l => l.ToLink()).ToList();
        }
    }
}
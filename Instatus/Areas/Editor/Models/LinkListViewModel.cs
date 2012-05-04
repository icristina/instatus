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
using Instatus;

namespace Instatus.Areas.Editor.Models
{
    public class LinkListViewModel : BaseViewModel<Page>
    {
        [ScaffoldColumn(true)]
        [Display(Order = 5)]
        public IEnumerable<LinkViewModel> Links { get; set; }

        public override void Load(Page model)
        {
            Links = model.Document.Links.Select(l =>
            {
                var viewModel = new LinkViewModel();
                viewModel.Load(l);
                return viewModel;
            })
            .ToList()
            .Pad(10);
        }

        public override void Save(Page model)
        {
            model.Document.Links = Links.RemoveNullOrEmpty().Select(l => l.ToModel()).ToList();
        }
    }
}
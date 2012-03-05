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
    public class CreativeViewModel<T> : BaseViewModel<T, IDataContext> where T : Page
    {
        [ScaffoldColumn(true)]
        [Display(Order = 5)]
        public IEnumerable<LinkViewModel> Links { get; set; }

        public override void Load(T model)
        {
            base.Load(model);

            Links = model.Document.Links.Select(l => new LinkViewModel(l.Title, l.Uri, l.Picture)).ToList().Pad(10);
        }

        public override void Save(T model)
        {
            base.Save(model);

            model.Document.Links = Links.RemoveNullOrEmpty().Select(l => l.ToWebLink()).ToList();
        }
    }
}
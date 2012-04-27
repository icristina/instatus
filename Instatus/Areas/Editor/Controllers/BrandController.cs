using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Models;
using Instatus.Data;
using Instatus.Controllers;
using Instatus.Web;
using Instatus.Services;
using System.IO;
using Instatus;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Instatus.Entities;

namespace Instatus.Areas.Editor.Controllers
{
    public class BrandViewModel : BaseViewModel<Page, IApplicationModel>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Href { get; set; }

        public string Visual { get; set; }

        public override void Load(Page model)
        {
            base.Load(model);

            Href = model.Document.Links.Uri(WebContentType.Html);
            Visual = model.Document.Links.Uri(WebContentType.Jpeg);
        }

        public override void Save(Page model)
        {
            base.Save(model);

            model.Document.Links.Clear();
            model.Document.Links.Add(new Link(WebConstant.ContentType.Html, Href));

            if (!Visual.IsEmpty())
                model.Document.Links.Add(new Link(WebConstant.ContentType.Jpg, Visual));
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Brands")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class BrandController : ScaffoldController<BrandViewModel, Page, IApplicationModel, int>
    {
        public override IEnumerable<Page> Query(IEnumerable<Page> set, Query query)
        {
            return set.Where(p => p.Kind == "Brand").ByAlphabetical();
        }

        public override Page CreateModelInstance()
        {
            return new Page(Kind.Brand);
        }
    }
}

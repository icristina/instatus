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

            Href = model.Links.Uri(WebContentType.Html);
            Visual = model.Links.Uri(WebContentType.Jpeg);
        }

        public override void Save(Page model)
        {
            base.Save(model);

            model.Links.ToList().ForEach(l => Context.Links.Remove(l));

            model.Links = new List<Link>()
            {
                new Link(WebContentType.Html, Href)
            };

            if (!Visual.IsEmpty())
                model.Links.Add(new Link(WebContentType.Jpeg, Visual));
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Brands")]
    public class BrandController : ScaffoldController<BrandViewModel, Brand, IApplicationModel, int>
    {
        public override IEnumerable<Brand> Query(IEnumerable<Brand> set, WebQuery query)
        {
            return set.ByAlphabetical();
        }
    }
}

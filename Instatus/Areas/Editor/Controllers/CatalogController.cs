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
using System.ComponentModel.DataAnnotations.Schema;
using Instatus.Areas.Editor.Models;

namespace Instatus.Areas.Editor.Controllers
{
    public class CatalogViewModel : PageViewModel
    {
        [Required]
        public string Name { get; set; }

        [Column("Catalog")]
        [Display(Name = "Category")]
        [AdditionalMetadata("Required", false)]
        public SelectList CatalogList { get; set; }

        [ScaffoldColumn(false)]
        public int? Catalog { get; set; }

        public override void Load(Page model)
        {
            base.Load(model);

            Catalog = ParentId(model, Kind.Catalog);
        }

        public override void Save(Page model)
        {
            base.Save(model);

            SaveAssociation(model, Kind.Catalog, Catalog);
        }

        public override void Databind()
        {
            base.Databind();

            CatalogList = SelectByKind(Kind.Catalog, Catalog);
        }
    }

    [Authorize(Roles = WebConstant.Role.Editor)]
    [Description("Catalogs")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class CatalogController : ScaffoldController<CatalogViewModel, Page, IApplicationModel, int>
    {
        public override IEnumerable<Page> Query(IEnumerable<Page> set, Query query)
        {
            return set.Where(p => p.Kind == "Catalog").ByAlphabetical();
        }

        public override Page CreateModelInstance()
        {
            return new Page(Kind.Catalog);
        }
    }
}

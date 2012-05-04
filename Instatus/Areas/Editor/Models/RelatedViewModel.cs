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
using Instatus;
using Instatus.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Instatus.Areas.Editor.Models
{
    public class RelatedViewModel : PageViewModel
    {
        [Column("Catalog")]
        [Display(Name = "Catalog", Order = 1)]
        public SelectList CatalogList { get; set; }

        [ScaffoldColumn(false)]
        public int? Catalog { get; set; }

        [Column("Region")]
        [Display(Name = "Region", Order = 2)]
        public SelectList RegionList { get; set; }

        [ScaffoldColumn(false)]
        public int? Region { get; set; }

        [Column("Brand")]
        [Display(Name = "Brand", Order = 3)]
        public SelectList BrandList { get; set; }

        [ScaffoldColumn(false)]
        public int? Brand { get; set; }

        public override void Load(Page model)
        {
            base.Load(model);

            Catalog = ParentId(model, Kind.Catalog);
            Region = ParentId(model, Kind.Region);
            Brand = ParentId(model, Kind.Brand);
        }

        public override void Save(Page model)
        {
            base.Save(model);

            SaveAssociation(model, Kind.Catalog, Catalog);
            SaveAssociation(model, Kind.Region, Region);
            SaveAssociation(model, Kind.Brand, Brand);
        }

        public override void Databind()
        {
            CatalogList = SelectByKind(Kind.Catalog, Catalog);
            RegionList = SelectByKind(Kind.Region, Region);
            BrandList = SelectByKind(Kind.Brand, Brand);
        }
    }
}
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

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class RelatedViewModel<T> : BaseViewModel<T, IApplicationModel> where T : Page
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

        public override void Load(T model)
        {
            base.Load(model);

            Catalog = LoadAssociation<Page, Catalog>(model.Parents);
            Brand = LoadAssociation<Page, Brand>(model.Parents);
            Region = LoadAssociation<Page, Region>(model.Parents);
        }

        public override void Save(T model)
        {
            base.Save(model);

            model.Parents = SaveAssociation<Page, Catalog>(Context.Pages, model.Parents, Catalog);
            model.Parents = SaveAssociation<Page, Region>(Context.Pages, model.Parents, Catalog);
            model.Parents = SaveAssociation<Page, Brand>(Context.Pages, model.Parents, Brand);
        }

        public override void Databind()
        {
            CatalogList = DatabindSelectList<Page, Catalog>(Context.Pages, Catalog);
        }
    }
}
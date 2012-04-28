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
    [ComplexType]
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

    public class PageViewModel : BaseViewModel<Page, IApplicationModel>
    {
        public int? ParentId(Page model, Kind kind)
        {
            var kindName = kind.ToString();
            var parentId = model.Parents.Where(p => p.Parent.Kind == kindName).Select(p => p.Id).FirstOrDefault();
            return parentId == 0 ? default(int?) : parentId;
        }

        public SelectList SelectByKind(Kind kind, int? selectedValue)
        {
            var kindName = kind.ToString();
            return new SelectList(Context.Pages.Where(p => p.Kind == kindName).Select(p => new
            {
                Id = p.Id,
                Name = p.Name
            }).ToList(), "Id", "Name", selectedValue);
        }

        public void SaveAssociation(Page model, Kind kind, int? selectedValue)
        {
            var kindName = kind.ToString();
            
            foreach (var association in Context.Associations.Where(a => a.ChildId == model.Id && a.Parent.Kind == kindName).ToList())
                Context.Associations.Remove(association);

            if (selectedValue.HasValue)
            {
                Context.Associations.Add(new Association()
                {
                    ParentId = selectedValue.Value,
                    ChildId = model.Id
                });
            }
        }
    }
}
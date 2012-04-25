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
    public class LocationViewModel<T> : BaseViewModel<T, IApplicationModel> where T : Place
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public double Zoom { get; set; }

        [Column("Region")]
        [Display(Name = "Region")]
        public SelectList RegionList { get; set; }

        [ScaffoldColumn(false)]
        public int? Region { get; set; }

        public override void Load(T model)
        {
            base.Load(model);
            
            Longitude = model.Point.Longitude;
            Latitude = model.Point.Latitude;
            Zoom = model.Point.Zoom;

            Region = LoadAssociation<Page, Region>(model.Parents);
        }

        public override void Save(T model)
        {
            base.Save(model);
            
            model.Point.Longitude = Longitude;
            model.Point.Latitude = Latitude;
            model.Point.Zoom = Zoom;

            model.Parents = SaveAssociation<Page, Region>(Context.Pages, model.Parents, Region);
        }

        public override void Databind() {
            RegionList = DatabindSelectList<Page, Region>(Context.Pages, Region);
        }
    }
}
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
using System.ComponentModel.DataAnnotations.Schema;
using Instatus.Entities;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class LocationViewModel : BaseViewModel<Page, IApplicationModel>
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        [Display(Name = "Map Zoom Level")]
        public double ZoomLevel { get; set; }

        //[Column("Region")]
        //[Display(Name = "Region")]
        //public SelectList RegionList { get; set; }

        //[ScaffoldColumn(false)]
        //public int? Region { get; set; }

        public override void Load(Page model)
        {
            base.Load(model);

            Longitude = model.Location.Longitude;
            Latitude = model.Location.Latitude;
            ZoomLevel = model.Location.ZoomLevel;

            //Region = LoadAssociation<Page, Region>(model.Parents);
        }

        public override void Save(Page model)
        {
            base.Save(model);
            
            model.Location.Longitude = Longitude;
            model.Location.Latitude = Latitude;
            model.Location.ZoomLevel = ZoomLevel;

            //model.Parents = SaveAssociation<Page, Region>(Context.Pages, model.Parents, Region);
        }

        public override void Databind() {
            //RegionList = DatabindSelectList<Page, Region>(Context.Pages, Region);
        }
    }
}
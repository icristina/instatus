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
    public class LocationViewModel<T> : BaseViewModel<T, BaseDataContext> where T : Place
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public double Zoom { get; set; }

        [Category("Overview")]
        [Column("Region")]
        [Display(Name = "Region")]
        public SelectList RegionList { get; set; }

        [ScaffoldColumn(false)]
        public int Region { get; set; }

        public override void Load(T model)
        {
            Longitude = model.Point.Longitude;
            Latitude = model.Point.Latitude;
            Zoom = model.Point.Zoom;
        }

        public override void Save(T model)
        {
            model.Point.Longitude = Longitude;
            model.Point.Latitude = Latitude;
            model.Point.Zoom = Zoom;
        }

        public override void Databind() {
            RegionList = new SelectList(Context.Pages.OfType<Region>(), "Id", "Name", Region);
        }
    }
}
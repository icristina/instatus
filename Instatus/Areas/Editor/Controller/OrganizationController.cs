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

namespace Instatus.Areas.Editor.Controllers
{
    public class OrganizationViewModel : BaseViewModel<Organization, BaseDataContext>
    {
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Zoom { get; set; }
        public int Priority { get; set; }
        public string Picture { get; set; }
        public string Data { get; set; }

        [Column("Region")]
        [Display(Name = "Region")]
        public SelectList RegionList { get; set; }

        [ScaffoldColumn(false)]
        public int Region { get; set; }

        [Column("Catalog")]
        [Display(Name = "Catalog")]
        public SelectList CatalogList { get; set; }

        [ScaffoldColumn(false)]
        public int Catalog { get; set; }

        public override void Load(Organization model)
        {
            base.Load(model);

            Longitude = model.Point.Longitude;
            Latitude = model.Point.Latitude;
            Zoom = model.Point.Zoom;
        }

        public override void Save(Organization model)
        {
            base.Save(model);

            model.Point.Longitude = Longitude;
            model.Point.Latitude = Latitude;
            model.Point.Zoom = Zoom;
        }

        public override void Databind()
        {
            RegionList = new SelectList(Context.Pages.OfType<Region>(), "Id", "Name", Catalog);
            CatalogList = new SelectList(Context.Pages.OfType<Catalog>(), "Id", "Name", Catalog);
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Organizations")]
    public class OrganizationController : ScaffoldController<OrganizationViewModel, Organization, BaseDataContext, int>
    {
        public override IOrderedQueryable<Organization> Query(IDbSet<Organization> set, WebQuery query)
        {
            return set.OrderBy(o => o.Name);
        }

        public override void ConfigureWebView(WebView<Organization> webView)
        {
            webView.Permissions = WebRole.Editor.ToPermissions();
            base.ConfigureWebView(webView);            
        }
    }
}

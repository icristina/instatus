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
using Instatus.Areas.Editor.Models;

namespace Instatus.Areas.Editor.Controllers
{
    public class OrganizationViewModel : BaseViewModel<Organization, BaseDataContext>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel Overview { get; set; }

        [Category("Overview")]
        [Column("Catalog")]
        [Display(Name = "Catalog")]
        public SelectList CatalogList { get; set; }

        [ScaffoldColumn(false)]
        public int Catalog { get; set; }

        [Category("Meta Tags")]
        public MetaTagsViewModel MetaTags { get; set; }

        [Category("Publishing")]
        public PublishingViewModel Publishing { get; set; }

        public override void Databind()
        {
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

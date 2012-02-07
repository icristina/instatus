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
    public class CatalogViewModel : BaseViewModel<Catalog>
    {
        public string Name { get; set; }
    }

    [Authorize(Roles = "Editor")]
    [Description("Catalogs")]
    public class CatalogController : ScaffoldController<CatalogViewModel, Catalog, BaseDataContext, int>
    {
        public override IOrderedQueryable<Catalog> Query(IDbSet<Catalog> set, WebQuery query)
        {
            return set.OrderBy(o => o.Name);
        }

        public override void ConfigureWebView(WebView<Catalog> webView)
        {
            webView.Permissions = WebRole.Editor.ToPermissions();
            base.ConfigureWebView(webView);            
        }
    }
}

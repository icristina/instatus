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
    public class BrandViewModel : BaseViewModel<Brand, BaseDataContext>
    {
        public string Name { get; set; }
        public string Data { get; set; }
        public string Href { get; set; }
        public string Visual { get; set; }

        public override void Load(Brand model)
        {
            base.Load(model);

            Href = model.Links.Uri(WebContentType.Html);
            Visual = model.Links.Uri(WebContentType.Jpeg);
        }

        public override void Save(Brand model)
        {
            base.Save(model);

            Context.MarkDeleted(model.Links);

            model.Links = new List<Link>()
            {
                new Link(WebContentType.Html, Href)
            };

            if (!Visual.IsEmpty())
                model.Links.Add(new Link(WebContentType.Jpeg, Visual));
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Brands")]
    public class BrandController : ScaffoldController<BrandViewModel, Brand, BaseDataContext, int>
    {
        public override IOrderedQueryable<Brand> Query(IDbSet<Brand> set, WebQuery query)
        {
            return set.OrderBy(o => o.Name);
        }

        public override void ConfigureWebView(WebView<Brand> webView)
        {
            webView.Permissions = WebRole.Editor.ToPermissions();
            base.ConfigureWebView(webView);            
        }
    }
}

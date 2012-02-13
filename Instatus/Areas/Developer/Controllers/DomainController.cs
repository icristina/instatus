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
using System.ComponentModel.DataAnnotations;

namespace Instatus.Areas.Developer.Controllers
{
    public class DomainViewModel : BaseViewModel<Domain, IDataContext>
    {
        public string Environment { get; set; }
        public string Uri { get; set; }

        [Column("ApplicationId")]
        [Display(Name = "Application")]
        public SelectList ApplicationList { get; set; }

        [ScaffoldColumn(false)]
        public int ApplicationId { get; set; }

        public override void Databind()
        {
            ApplicationList = new SelectList(Context.Pages.OfType<Application>().ToList(), "Id", "Name", ApplicationId);
        }
    }
    
    [Authorize(Roles = "Administrator")]
    public class DomainController : ScaffoldController<DomainViewModel, Domain, IDataContext, int>
    {
        public override void ConfigureWebView(WebView<Domain> webView)
        {
            webView.Permissions = WebRole.Administrator.ToPermissions();
            base.ConfigureWebView(webView);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            WebApp.Reset();
        }
    }
}

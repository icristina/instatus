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

namespace Instatus.Areas.Microsite.Controllers
{
    public class DomainViewModel : BaseViewModel<Domain, BaseDataContext>
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
            ApplicationList = new SelectList(Context.Applications.ToList(), "Id", "Name", ApplicationId);
        }
    }
    
    [Authorize(Roles = "Administrator")]
    public class DomainController : ScaffoldController<DomainViewModel, Domain, BaseDataContext, int>
    {
        public override void ConfigureWebView(WebView<Domain> webView)
        {
            webView.Permissions = WebRole.Administrator.ToPermissions();
            base.ConfigureWebView(webView);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            PubSub.Provider.Publish<ApplicationReset>(new ApplicationReset());
        }
    }
}

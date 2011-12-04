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
using System.Data.Entity;

namespace Instatus.Areas.Developer.Controllers
{
    public class CredentialViewModel : BaseViewModel<Credential, BaseDataContext>
    {
        public string Name { get; set; }
        public string Uri { get; set; }

        [Display(Name = "Secret")]
        public string UpdatedSecret { get; set; }
        
        public string Environment { get; set; }
        public string Scope { get; set; }
        public string Provider { get; set; }

        [Column("ApplicationId")]
        [Display(Name = "Application")]
        public SelectList ApplicationList { get; set; }

        [ScaffoldColumn(false)]
        public int ApplicationId { get; set; }

        public override void Databind()
        {
            ApplicationList = new SelectList(Context.Applications.ToList(), "Id", "Name", ApplicationId);
        }

        public override void Save(Credential model)
        {
            if (!UpdatedSecret.IsEmpty())
                model.Secret = UpdatedSecret;            
            
            base.Save(model);
        }
    }
    
    [Authorize(Roles = "Administrator")]
    public class CredentialController : ScaffoldController<CredentialViewModel, Credential, BaseDataContext, int>
    {
        public override IOrderedQueryable<Credential> Query(IDbSet<Credential> set, WebQuery query)
        {
            return set.Where(c => c.Application != null).OrderBy(c => c.CreatedTime);
        }
        
        public override void ConfigureWebView(WebView<Credential> webView)
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

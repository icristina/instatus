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
using System.ComponentModel;

namespace Instatus.Areas.Developer.Controllers
{
    public class CredentialViewModel : BaseViewModel<Credential, IApplicationContext>
    {
        public string Name { get; set; }
        
        [Required]
        public string Uri { get; set; }

        [Display(Name = "Secret")]
        public string UpdatedSecret { get; set; }
        
        [Required]
        public string Environment { get; set; }
        public string Scope { get; set; }

        [Required]
        public string Provider { get; set; }

        public string Features { get; set; }

        [Column("ApplicationId")]
        [Display(Name = "Application")]
        [AdditionalMetadata("Required", true)]
        public SelectList ApplicationList { get; set; }

        [ScaffoldColumn(false)]
        public int ApplicationId { get; set; }

        public override void Databind()
        {
            ApplicationList = DatabindSelectList<Page, Application>(Context.Pages, ApplicationId);
        }

        public override void Save(Credential model)
        {
            base.Save(model);
            
            if (!UpdatedSecret.IsEmpty())
                model.Secret = UpdatedSecret;                                    
        }
    }
    
    [Authorize(Roles = "Administrator")]
    [Description("Credentials")]
    public class CredentialController : ScaffoldController<CredentialViewModel, Credential, IApplicationContext, int>
    {
        public override IEnumerable<Credential> Query(IEnumerable<Credential> set, WebQuery query)
        {
            return set
                .AsQueryable()
                .Where(c => c.Application != null)
                .OrderBy(c => c.CreatedTime);
        }       

        public override void SaveChanges()
        {
            base.SaveChanges();
            WebApp.Reset();
        }
    }
}

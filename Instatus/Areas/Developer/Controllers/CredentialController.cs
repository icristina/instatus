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
using Instatus.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Instatus.Areas.Developer.Controllers
{
    public class CredentialViewModel : BaseViewModel<Credential, IApplicationModel>
    {
        public string Name { get; set; }
        
        [Required]
        public string ApiKey { get; set; }

        [Display(Name = "Secret")]
        public string UpdatedApplicationSecret { get; set; }
        
        [Required]
        public string Deployment { get; set; }
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
            ApplicationList = new SelectList(Context.Applications.ToList(), "Id", "Name", ApplicationId);
        }

        public override void Save(Credential model)
        {
            base.Save(model);

            if (!UpdatedApplicationSecret.IsEmpty())
                model.AppSecret = UpdatedApplicationSecret;                                  
        }
    }
    
    [Authorize(Roles = WebConstant.Role.Developer)]
    [Description("Credentials")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class CredentialController : ScaffoldController<CredentialViewModel, Credential, IApplicationModel, int>
    { 
        public override void SaveChanges()
        {
            base.SaveChanges();
            WebApp.Reset();
        }
    }
}

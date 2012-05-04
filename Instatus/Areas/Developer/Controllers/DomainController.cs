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
using Instatus.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Instatus.Areas.Developer.Controllers
{
    public class DomainViewModel : BaseViewModel<Domain, IApplicationModel>
    {
        public string Environment { get; set; }
        public string Hostname { get; set; }
        public bool Canonical { get; set; }

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
    
    [Authorize(Roles = WebConstant.Role.Developer)]
    [Description("Domains")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class DomainController : ScaffoldController<DomainViewModel, Domain, IApplicationModel, int>
    {
        public override void SaveChanges()
        {
            base.SaveChanges();
            WebApp.Reset();
        }
    }
}

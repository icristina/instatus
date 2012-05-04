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
using System.ComponentModel;
using System.Data.Entity;
using Instatus.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Instatus;

namespace Instatus.Areas.Developer.Controllers
{
    public class RedirectViewModel : BaseViewModel<Redirect, IApplicationModel>
    {
        [Required]
        public string Source { get; set; }

        [Required]
        public string Location { get; set; }

        [Column("HttpStatusCode")]
        [Display(Name = "Http Code")]
        [AdditionalMetadata("Required", true)]
        public SelectList HttpStatusCodeList { get; set; }

        [ScaffoldColumn(false)]
        public int HttpStatusCode { get; set; }

        public override void Databind()
        {
            HttpStatusCodeList = new SelectList(new Dictionary<int, string>() { { 301, "Permanent Redirect" }, { 302, "Temporary Redirect" } }, "Key", "Value", HttpStatusCode);
        }
    }
    
    [Authorize(Roles = WebConstant.Role.Developer)]
    [Description("Redirects")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class RedirectController : ScaffoldController<RedirectViewModel, Redirect, IApplicationModel, int>
    {
        public override void SaveChanges()
        {
            base.SaveChanges();
            WebApp.Reset();
        }
    }
}

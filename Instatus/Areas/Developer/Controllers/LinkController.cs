﻿using System;
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

namespace Instatus.Areas.Developer.Controllers
{
    public class LinkViewModel : BaseViewModel<Link, IApplicationModel>
    {
        [Required]
        public string Uri { get; set; }

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
    
    [Authorize(Roles = "Administrator")]
    [Description("Redirects")]
    public class LinkController : ScaffoldController<LinkViewModel, Link, IApplicationModel, int>
    {
        public override IEnumerable<Link> Query(IEnumerable<Link> set, WebQuery query)
        {
            return set
                .AsQueryable()
                .Redirects();
        }
        
        public override void SaveChanges()
        {
            base.SaveChanges();
            WebApp.Reset();
        }
    }
}

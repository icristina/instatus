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
using System.Data.Entity;
using System.ComponentModel;
using Instatus.Entities;

namespace Instatus.Areas.Developer.Controllers
{
    public class PhraseViewModel : BaseViewModel<Phrase>
    {
        public int Locale { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
    }

    [Authorize(Roles = WebConstant.Role.Developer)]
    [Description("Localization")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class PhraseController : ScaffoldController<PhraseViewModel, Phrase, IApplicationModel, int>
    {
        public override IEnumerable<Phrase> Query(IEnumerable<Phrase> set, Query query)
        {
            return set.OrderBy(o => o.Name);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            WebApp.Reset();
        }
    }
}

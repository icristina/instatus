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
    public class PhraseViewModel : BaseViewModel<Phrase>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
    }

    [Authorize(Roles = "Developer")]
    [Description("Localization")]
    public class PhraseController : ScaffoldController<PhraseViewModel, Phrase, BaseDataContext, int>
    {
        public override IEnumerable<Phrase> Query(IEnumerable<Phrase> set, WebQuery query)
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

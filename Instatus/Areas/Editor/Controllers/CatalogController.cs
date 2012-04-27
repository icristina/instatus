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
using Instatus;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Instatus.Entities;

namespace Instatus.Areas.Editor.Controllers
{
    public class CatalogViewModel : BaseViewModel<Page>
    {
        [Required]
        public string Name { get; set; }
    }

    [Authorize(Roles = "Editor")]
    [Description("Catalogs")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class CatalogController : ScaffoldController<CatalogViewModel, Page, IApplicationModel, int>
    {
        public override IEnumerable<Page> Query(IEnumerable<Page> set, Query query)
        {
            return set.Where(p => p.Kind == "Catalog").ByAlphabetical();
        }

        public override Page CreateModelInstance()
        {
            return new Page(Kind.Catalog);
        }
    }
}

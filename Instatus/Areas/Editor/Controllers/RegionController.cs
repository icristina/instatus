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

namespace Instatus.Areas.Editor.Controllers
{
    public class RegionViewModel : BaseViewModel<Region>
    {
        public string Name { get; set; }
    }

    [Authorize(Roles = "Editor")]
    [Description("Regions")]
    public class RegionController : ScaffoldController<RegionViewModel, Region, BaseDataContext, int>
    {
        public override IEnumerable<Region> Query(IDbSet<Region> set, WebQuery query)
        {
            return set.ByAlphabetical();
        }
    }
}

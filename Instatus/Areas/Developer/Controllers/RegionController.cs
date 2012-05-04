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
using Instatus.Areas.Editor.Models;
using Instatus.Entities;

namespace Instatus.Areas.Developer.Controllers
{
    public class RegionViewModel : BaseViewModel<Page, IApplicationModel>
    {
        [Category("Overview")]
        [Display(Order = 1)]      
        [Required]
        public string Name { get; set; }

        [Category("Location")]
        public LocationViewModel Location { get; set; }

        [Category("Publishing")]
        public PublishingViewModel Publishing { get; set; }

        public RegionViewModel()
        {
            Location = new LocationViewModel();
            Publishing = new PublishingViewModel();
        }
    }

    [Authorize(Roles = WebConstant.Role.Developer)]
    [Description("Regions")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class RegionController : ScaffoldController<RegionViewModel, Page, IApplicationModel, int>
    {
        public override IEnumerable<Page> Query(IEnumerable<Page> set, Query query)
        {
            return set.Where(p => p.Kind == "Region").ByAlphabetical();
        }

        public override Page CreateModelInstance()
        {
            return new Page(Kind.Region);
        }
    }
}

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

namespace Instatus.Areas.Developer.Controllers
{
    public class RegionViewModel : BaseViewModel<Region, IDataContext>
    {
        [Category("Overview")]
        [Display(Order = 1)]      
        [Required]
        public string Name { get; set; }

        [Category("Location")]
        public LocationViewModel<Region> Location { get; set; }

        [Category("Publishing")]
        public PublishingViewModel<Region> Publishing { get; set; }

        public RegionViewModel()
        {
            Location = new LocationViewModel<Region>();
            Publishing = new PublishingViewModel<Region>();
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Regions")]
    public class RegionController : ScaffoldController<RegionViewModel, Region, IDataContext, int>
    {
        public override IEnumerable<Region> Query(IEnumerable<Region> set, WebQuery query)
        {
            return set.ByAlphabetical();
        }
    }
}

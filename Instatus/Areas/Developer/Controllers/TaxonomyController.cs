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

namespace Instatus.Areas.Developer.Controllers
{
    public class TaxonomyViewModel : BaseViewModel<Taxonomy>
    {
        [Required]
        public string Name { get; set; }
    }
    
    [Authorize(Roles = "Developer")]
    [Description("Taxonomies")]
    public class TaxonomyController : ScaffoldController<TaxonomyViewModel, Taxonomy, IDataContext, int>
    {

    }
}

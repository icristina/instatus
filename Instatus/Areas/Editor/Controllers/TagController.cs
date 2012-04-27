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
using Instatus.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Instatus.Areas.Editor.Controllers
{
    public class TagViewModel : BaseViewModel<Tag, IApplicationModel>
    {
        [Required]
        public string Name { get; set; }

        //[Column("TaxonomyId")]
        //[Display(Name = "Taxonomy")]
        //public SelectList TaxonomyList { get; set; }

        //[ScaffoldColumn(false)]
        //public int? TaxonomyId { get; set; }

        public override void Databind()
        {
            //TaxonomyList = new SelectList(Context.Taxonomies.ToList(), "Id", "Name", TaxonomyId);
        }
    }
    
    [Authorize(Roles = "Editor")]
    [Description("Tags")]
    public class TagController : ScaffoldController<TagViewModel, Tag, IApplicationModel, int>
    {

    }
}

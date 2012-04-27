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

namespace Instatus.Areas.Editor.Controllers
{
    public class ProfileViewModel : BaseViewModel<Page, IApplicationModel>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel Overview { get; set; }

        //[Category("Overview")]
        //[Column("Catalog")]
        //[Display(Name = "Category", Order = 2)]
        //public SelectList CatalogList { get; set; }

        //[ScaffoldColumn(false)]
        //public int? Catalog { get; set; }

        [Category("Publishing")]
        [Display(Order = 3)]
        public PublishingViewModel Publishing { get; set; }

        public override void Load(Page model)
        {
            base.Load(model);

            //Catalog = LoadAssociation<Page, Catalog>(model.Parents);
        }

        public override void Save(Page model)
        {
            base.Save(model);

            //model.Parents = SaveAssociation<Page, Catalog>(Context.Pages, model.Parents, Catalog);
        }

        public override void Databind()
        {
            base.Databind();

            //CatalogList = DatabindSelectList<Page, Catalog>(Context.Pages, Catalog);
        }

        public ProfileViewModel()
        {
            Overview = new OverviewViewModel();
            Publishing = new PublishingViewModel();
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Profiles")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class ProfileController : ScaffoldController<ProfileViewModel, Page, IApplicationModel, int>
    {
        public override IEnumerable<Page> Query(IEnumerable<Page> set, Query query)
        {
            return set.Where(p => p.Kind == "Profile").ByAlphabetical();
        }

        public override Page CreateModelInstance()
        {
            return new Page(Kind.Profile);
        }
    }
}

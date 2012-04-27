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
    public class OrganizationViewModel : BaseViewModel<Page, IApplicationModel>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel Overview { get; set; }

        //[Category("Overview")]
        //[Column("Catalog")]
        //[Display(Name = "Category")]       
        //[AdditionalMetadata("Required", true)]
        //public SelectList CatalogList { get; set; }

        //[ScaffoldColumn(false)]
        //public int Catalog { get; set; }

        [Category("Location")]
        public LocationViewModel Location { get; set; }

        [Category("Meta Tags")]
        public MetaTagsViewModel MetaTags { get; set; }

        [Category("Publishing")]
        public PublishingViewModel Publishing { get; set; }

        public override void Load(Page model)
        {
            base.Load(model);

            //Catalog = LoadAssociation<Page, Catalog>(model.Parents) ?? 0;
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

        public OrganizationViewModel()
        {
            Overview = new OverviewViewModel();
            Location = new LocationViewModel();
            MetaTags = new MetaTagsViewModel();
            Publishing = new PublishingViewModel();
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Organizations")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class OrganizationController : ScaffoldController<OrganizationViewModel, Page, IApplicationModel, int>
    {
        public override IEnumerable<Page> Query(IEnumerable<Page> set, Query query)
        {
            return set.Where(p => p.Kind == "Organization").ByAlphabetical();
        }

        public override Page CreateModelInstance()
        {
            return new Page(Kind.Organization);
        }
    }
}

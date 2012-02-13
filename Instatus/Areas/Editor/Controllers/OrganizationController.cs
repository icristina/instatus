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

namespace Instatus.Areas.Editor.Controllers
{
    public class OrganizationViewModel : BaseViewModel<Organization, IDataContext>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel<Organization> Overview { get; set; }

        [Category("Overview")]
        [Column("Catalog")]
        [Display(Name = "Category")]       
        [AdditionalMetadata("Required", true)]
        public SelectList CatalogList { get; set; }

        [ScaffoldColumn(false)]
        public int Catalog { get; set; }

        [Category("Location")]
        public LocationViewModel<Organization> Location { get; set; }

        [Category("Meta Tags")]
        public MetaTagsViewModel<Organization> MetaTags { get; set; }

        [Category("Publishing")]
        public PublishingViewModel<Organization> Publishing { get; set; }

        public override void Load(Organization model)
        {
            base.Load(model);

            model.Parents.OfType<Catalog>().ForFirst(c => Catalog = c.Id);
        }

        public override void Save(Organization model)
        {
            base.Save(model);

            model.Parents.OfType<Catalog>().ForFirst(c => model.Parents.Remove(c));
            model.Parents.Add(Context.Pages.Find(Catalog));
        }

        public override void Databind()
        {
            base.Databind();

            CatalogList = new SelectList(Context.Pages.OfType<Catalog>(), "Id", "Name", Catalog);
        }

        public OrganizationViewModel()
        {
            Overview = new OverviewViewModel<Organization>();
            Location = new LocationViewModel<Organization>();
            MetaTags = new MetaTagsViewModel<Organization>();
            Publishing = new PublishingViewModel<Organization>();
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Organizations")]
    public class OrganizationController : ScaffoldController<OrganizationViewModel, Organization, IDataContext, int>
    {
        public override IEnumerable<Organization> Query(IEnumerable<Organization> set, WebQuery query)
        {
            return set.ByAlphabetical();
        }
    }
}

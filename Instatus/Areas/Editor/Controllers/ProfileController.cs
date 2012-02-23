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
    public class ProfileViewModel : BaseViewModel<Profile, IDataContext>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel<Profile> Overview { get; set; }

        [Category("Overview")]
        [Column("Catalog")]
        [Display(Name = "Category", Order = 2)]
        public SelectList CatalogList { get; set; }

        [ScaffoldColumn(false)]
        public int? Catalog { get; set; }

        [Category("Publishing")]
        [Display(Order = 3)]
        public PublishingViewModel<Profile> Publishing { get; set; }

        public override void Load(Profile model)
        {
            base.Load(model);

            model.Parents.OfType<Catalog>().ForFirst(c => Catalog = c.Id);
        }

        public override void Save(Profile model)
        {
            base.Save(model);

            model.Parents.OfType<Catalog>().ForFirst(c => model.Parents.Remove(c));
            
            if (Catalog.HasValue) {
                model.Parents.Add(Context.Pages.Find(Catalog));
            }
        }

        public override void Databind()
        {
            base.Databind();

            CatalogList = new SelectList(Context.Pages.OfType<Catalog>(), "Id", "Name", Catalog);
        }

        public ProfileViewModel()
        {
            Overview = new OverviewViewModel<Profile>();
            Publishing = new PublishingViewModel<Profile>();
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Profiles")]
    public class ProfileController : ScaffoldController<ProfileViewModel, Profile, IDataContext, int>
    {
        public override IEnumerable<Profile> Query(IEnumerable<Profile> set, WebQuery query)
        {
            return set.ByAlphabetical();
        }
    }
}

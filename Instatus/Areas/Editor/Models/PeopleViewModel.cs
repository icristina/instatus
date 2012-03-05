using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using System.Web.Mvc;
using Instatus.Models;
using Instatus.Data;
using Instatus;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class PeopleViewModel<T> : BaseViewModel<T, IApplicationContext> where T : Page
    {
        [Column("Profiles")]
        [Display(Name = "People", Order = 2)]
        public MultiSelectList ProfilesList { get; set; }

        [ScaffoldColumn(false)]
        public int[] Profiles { get; set; }

        public override void Load(T model)
        {
            base.Load(model);

            Profiles = model.Parents.OfType<Profile>().Select(p => p.Id).ToArray();
        }

        public override void Save(T model)
        {
            base.Save(model);

            model.Parents.RemoveAll<Profile, Page>();

            if (!Profiles.IsEmpty())
            {
                foreach (var profileId in Profiles)
                {
                    model.Parents.Add(Context.Pages.Find(profileId));
                }
            }
        }

        public override void Databind()
        {
            base.Databind();

            ProfilesList = DatabindMultiSelectList<Page, Profile>(Context.Pages, Profiles);
        }
    }
}
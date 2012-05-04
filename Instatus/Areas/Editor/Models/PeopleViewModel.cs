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
using System.ComponentModel.DataAnnotations.Schema;
using Instatus.Entities;

namespace Instatus.Areas.Editor.Models
{
    public class PeopleViewModel : PageViewModel
    {
        [Column("Profiles")]
        [Display(Name = "People", Order = 2)]
        public MultiSelectList ProfilesList { get; set; }

        [ScaffoldColumn(false)]
        public int[] Profiles { get; set; }

        public override void Load(Page model)
        {
            base.Load(model);

            Profiles = model.Parents.Where(p => p.Parent.Kind == "Profile").Select(p => p.ParentId).ToArray();
        }

        public override void Save(Page model)
        {
            base.Save(model);

            foreach (var association in model.Parents.Where(p => p.Parent.Kind == "Profile").ToList())
                Context.Associations.Remove(association);

            if (!Profiles.IsEmpty())
            {
                foreach (var profileId in Profiles)
                {
                    model.Parents.Add(new Association()
                    {
                        ParentId = profileId
                    });
                }
            }
        }

        public override void Databind()
        {
            base.Databind();

            ProfilesList = new MultiSelectList(Context.Pages.Where(p => p.Kind == "Profile").Select(p => new
            {
                Id = p.Id,
                Name = p.Name
            }).ToList(), "Id", "Name", Profiles);
        }
    }
}
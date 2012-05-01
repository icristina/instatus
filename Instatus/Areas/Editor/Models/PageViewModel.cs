using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Entities;
using Instatus.Models;
using Instatus.Web;

namespace Instatus.Areas.Editor.Models
{
    public class PageViewModel : BaseViewModel<Page, IApplicationModel>
    {
        public int? ParentId(Page model, Kind kind)
        {
            var kindName = kind.ToString();
            var parentId = model.Parents.Where(p => p.Parent.Kind == kindName).Select(p => p.ParentId).FirstOrDefault();
            return parentId == 0 ? default(int?) : parentId;
        }

        public SelectList SelectByKind(Kind kind, object selectedValue)
        {
            var kindName = kind.ToString();
            return new SelectList(Context.Pages.Where(p => p.Kind == kindName).Select(p => new
            {
                Id = p.Id,
                Name = p.Name
            }).ToList(), "Id", "Name", selectedValue);
        }

        public void SaveAssociation(Page model, Kind kind, int? selectedValue)
        {
            var kindName = kind.ToString();

            foreach (var association in Context.Associations.Where(a => a.ChildId == model.Id && a.Parent.Kind == kindName).ToList())
                Context.Associations.Remove(association);

            if (selectedValue.HasValue)
            {
                Context.Associations.Add(new Association()
                {
                    ParentId = selectedValue.Value,
                    ChildId = model.Id
                });
            }
        }
    }
}
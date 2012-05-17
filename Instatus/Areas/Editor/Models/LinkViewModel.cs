using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using System.ComponentModel.DataAnnotations;
using Instatus.Web;
using Instatus.Models;

namespace Instatus.Areas.Editor.Models
{
    public class LinkViewModel : BaseViewModel<Link>, IHasValue
    {
        public string Name { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Uri { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Picture { get; set; }

        [ScaffoldColumn(false)]
        public bool HasValue
        {
            get
            {
                return !Uri.IsEmpty() && !Name.IsEmpty();
            }
        }

        public LinkViewModel() { }

        public override void Load(Link model)
        {
            base.Load(model);

            Name = model.Title;
        }

        public override void Save(Link model)
        {
            base.Save(model);

            model.Title = Name;
            model.Rel = WebConstant.Rel.Attachment;
        }
    }
}
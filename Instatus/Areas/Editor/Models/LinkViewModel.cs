using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Areas.Editor.Models
{
    public class LinkViewModel : IHasValue
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
    }
    
}
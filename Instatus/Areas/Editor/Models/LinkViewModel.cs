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

        public LinkViewModel() { }

        public LinkViewModel(string name, string uri, string picture)
        {
            Name = name;
            Uri = uri;
            Picture = picture;
        }

        public Link ToLink()
        {
            return new Link()
            {
                Uri = Uri,
                Title = Name,
                Picture = Picture
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using Instatus.Models;
using Instatus.Data;
using System.Web.Mvc;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class OverviewViewModel<T> : BaseViewModel<T> where T : Page
    {
        [DisplayName("Title")]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        [AllowHtml]
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Picture { get; set; }
    }
}
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
using System.ComponentModel.DataAnnotations.Schema;
using Instatus.Entities;

namespace Instatus.Areas.Editor.Models
{
    public class OverviewViewModel : BaseViewModel<Page>
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
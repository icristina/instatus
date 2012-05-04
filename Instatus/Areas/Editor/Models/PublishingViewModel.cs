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
    public class PublishingViewModel : BaseViewModel<Page>
    {
        [DisplayName("Alias (for friendly url)")]
        [Required]
        [RegularExpression(WebConstant.RegularExpression.Alias, ErrorMessageResourceName = WebPhrase.ErrorMessage.InvalidFriendlyIdentifier, ErrorMessageResourceType = typeof(WebPhrase))]
        public string Alias { get; set; }      
       
        [DisplayName("Published Time")]
        public DateTime PublishedTime { get; set; }

        [Column("Published")]
        [Display(Name = "Published")]
        [AdditionalMetadata("Required", true)]
        public SelectList PublishedList { get; set; }

        [ScaffoldColumn(false)]
        public string Published { get; set; }

        public override void Databind()
        {
            PublishedList = WebUtility.CreateSelectList(new Published[] { Instatus.Models.Published.Active, Instatus.Models.Published.Draft }, Published, new string[] { "Active", "Draft" });
        }
    }
}
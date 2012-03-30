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
    public class PublishingViewModel<T> : BaseViewModel<T> where T : Page
    {
        [DisplayName("Friendly Url")]
        [Required]
        [RegularExpression(ValidationPatterns.Slug, ErrorMessageResourceName = WebPhrase.ErrorMessage.InvalidFriendlyIdentifier, ErrorMessageResourceType = typeof(WebPhrase))]
        public string Slug { get; set; }      
       
        public int Priority { get; set; }

        [DisplayName("Published Time")]
        public DateTime PublishedTime { get; set; }

        [Column("Status")]
        [Display(Name = "Status")]
        [AdditionalMetadata("Required", true)]
        public SelectList StatusList { get; set; }

        [ScaffoldColumn(false)]
        public string Status { get; set; }
        
        public string Data { get; set; }

        public string Category { get; set; }

        public override void Databind()
        {
            StatusList = WebUtility.CreateSelectList(new WebStatus[] { WebStatus.Published, WebStatus.Draft }, Status, new string[] { "Published", "Draft" });
        }
    }
}
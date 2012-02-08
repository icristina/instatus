using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Instatus.Web;
using Instatus.Models;
using Instatus.Data;

namespace Instatus.Areas.Editor.Models
{
    [ComplexType]
    public class PublishingViewModel<T> : BaseViewModel<T> where T : Page
    {
        [DisplayName("Friendly Url")]
        [Required]
        [RegularExpression(ValidationPatterns.Slug, ErrorMessage = ValidationMessages.InvalidSlug)]
        public string Slug { get; set; }      
       
        public int Priority { get; set; }
        
        public DateTime PublishedTime { get; set; }
        
        public string Data { get; set; }   
    }
}
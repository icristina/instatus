using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Instatus.Sample.Models
{
    public class SubscribeModel
    {
        [Display(Name = "Email address")]
        [Required]
        public string EmailAddress { get; set; }

        [Display(Name = "Subscribe for marketing emails")]
        public bool IsSubscribed { get; set; }
    }
}
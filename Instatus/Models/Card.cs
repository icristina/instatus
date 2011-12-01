using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Models
{
    [ComplexType]
    public class Card
    {
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string EmailAddress { get; set; }
    }
}
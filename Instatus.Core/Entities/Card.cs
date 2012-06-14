using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Instatus.Entities
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
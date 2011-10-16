using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Models
{
    [ComplexType]
    public class Name
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", GivenName, FamilyName);
        }
    }
}
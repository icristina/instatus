using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Profile : Page
    {
        public virtual Organization Organization { get; set; }
        public int? OrganizationId { get; set; }
    }
}
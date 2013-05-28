using System;
using System.Collections.Generic;
using System.Linq;

namespace Instatus.Entities
{
    public class Address
    {
        public string FormattedAddress { get; set; }
        public string StreetAddress { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
    }
}
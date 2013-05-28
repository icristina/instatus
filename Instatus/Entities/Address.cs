using System;
using System.Collections.Generic;
using System.Linq;

namespace Instatus.Entities
{
    public class Address
    {
        public string StreetAddress { get; set; }
        public string StreetAddress2 { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
    }
}
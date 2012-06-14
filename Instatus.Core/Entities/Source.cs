using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Entities
{
    [ComplexType]
    public class Source
    {
        public string Uri { get; set; }
        public string Provider { get; set; }
    }
}
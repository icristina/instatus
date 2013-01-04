using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Models
{
    public class Pagination
    {
        public object RouteValues { get; set; }
        public IPaged List { get; set; }
    }
}
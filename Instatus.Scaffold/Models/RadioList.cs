using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Scaffold.Models
{
    public class RadioList
    {
        public string Name { get; set; }
        public SelectList SelectList { get; set; }
    }
}
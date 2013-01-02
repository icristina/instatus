using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Models
{
    public class Button
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public object RouteData { get; set; }
        public string Text { get; set; }
        public string ClassName { get; set; }
    }
}
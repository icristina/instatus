using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Areas.Twitter
{
    public static class Bootstrap
    {
        public static List<string> Stylesheets = new List<string>() { "bootstrap.css", "bootstrap-responsive.css" };
        public static List<string> Scripts = new List<string>() { "modernizr.js", "jquery.js", "respond.js", "bootstrap.js" };

        public static void FixedWidth()
        {
            Scripts.Remove("respond.js");
            Stylesheets.Remove("bootstrap-responsive.css");
        }
    }
}
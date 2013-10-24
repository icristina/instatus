using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Instatus.Server
{
    public class MvcConfig
    {
        public static void RegisterConfiguration()
        {
            HtmlHelper.ClientValidationEnabled = false;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = false;
        }
    }
}

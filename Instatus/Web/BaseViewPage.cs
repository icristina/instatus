using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Instatus.Web
{
    // http://haacked.com/archive/2011/02/21/changing-base-type-of-a-razor-view.aspx
    public abstract class BaseViewPage<T> : WebViewPage<T>
    {
        public bool IsAdministrator
        {
            get
            {
                return User.IsInRole(WebRole.Administrator.ToString());
            }
        }

        public bool IsDebug
        {
            get
            {
                return HttpContext.Current.ApplicationInstance.IsDebug();
            }
        }
    }
}
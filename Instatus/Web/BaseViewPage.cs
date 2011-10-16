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
        public const string DetailsAction = "details";
        public const string IndexAction = "index";
        public const string CreateAction = "create";
        public const string DeleteAction = "delete";
        public const string EditAction = "edit";
        public const string FeedAction = "feed";

        public bool IsAdministrator
        {
            get
            {
                return User.IsInRole("Administrator");
            }
        }

        public bool IsDebug
        {
            get
            {
                return System.Diagnostics.Debugger.IsAttached;
            }
        }

        public object RouteId
        {
            get
            {
                var routeData = ViewContext.RouteData;
                return routeData.Values["id"] ?? routeData.Values["slug"];
            }
        }

        public string RouteClassNames
        {
            get
            {
                var routeData = ViewContext.RouteData;
                return string.Format("{0} {1} {2}",
                    routeData.AreaName().ToCamelCase(),
                    routeData.ControllerName().ToCamelCase(),
                    routeData.ActionName().ToCamelCase()).RemoveDoubleSpaces();                
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Specialized;

namespace Instatus.Web
{
    public interface IWebCommand
    {
        WebLink GetLink(dynamic viewModel, UrlHelper urlHelper);
        bool Execute(dynamic viewModel, RouteData routeData, NameValueCollection requestParams);
    }
}

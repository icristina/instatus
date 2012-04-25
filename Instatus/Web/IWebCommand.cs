using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Specialized;
using Instatus.Models;

namespace Instatus.Web
{
    public interface IWebCommand
    {
        string Name { get; }
        Link GetLink(dynamic viewModel, UrlHelper urlHelper);
        bool Execute(dynamic viewModel, UrlHelper urlHelper, RouteData routeData, NameValueCollection requestParams);
    }
}

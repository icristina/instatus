using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Data;
using Instatus.Web;
using Instatus.Models;
using System.Web.Routing;
using System.Collections.Specialized;
using Instatus;

namespace Instatus.Commands
{
    public class SelectCommand : IWebCommand
    {
        public string Name
        {
            get
            {
                return "Select";
            }
        }
        
        public WebLink GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            return new WebLink()
            {
                Title = "Select",
                Rel = "select"
            };           
        }

        public bool Execute(dynamic viewModel, UrlHelper urlHelper, RouteData routeData, NameValueCollection requestParams)
        {
            return true;
        }
    }
}

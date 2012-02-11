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
    public class GenericCommand : IWebCommand
    {
        private string name;
        private string title;
        
        public string Name
        {
            get
            {
                return name;
            }
        }
        
        public WebLink GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            return new WebLink()
            {
                Uri = viewModel is IResource ? viewModel.Uri : viewModel.GetKey(),
                Title = title ?? name.ToCapitalized(),
                Rel = name
            };           
        }

        public bool Execute(dynamic viewModel, UrlHelper urlHelper, RouteData routeData, NameValueCollection requestParams)
        {
            return true;
        }

        public GenericCommand(string name, string title = null)
        {
            this.name = name;
            this.title = title;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Data;
using Instatus.Entities;
using Instatus.Models;

namespace Instatus.Web
{
    public class DbSitemapProvider : SiteMapProvider
    {
        public override SiteMapNode FindSiteMapNode(string rawUrl)
        {
            throw new NotImplementedException();
        }

        public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
        {
            var applicationModel = DependencyResolver.Current.GetService<IApplicationModel>();

            return null;
        }

        public override SiteMapNode GetParentNode(SiteMapNode node)
        {
            throw new NotImplementedException();
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            var applicationModel = DependencyResolver.Current.GetService<IApplicationModel>();

            return null;
        }
    }
}
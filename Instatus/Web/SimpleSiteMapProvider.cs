using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Web.Routing;

namespace Instatus.Web
{
    public class SimpleSiteMapProvider : SiteMapProvider
    {       
        public override SiteMapNode FindSiteMapNode(string rawUrl)
        {
            return null;
        }

        public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
        {
            return null;
        }

        public override SiteMapNode GetParentNode(SiteMapNode node)
        {
            return null;
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return null;
        }
    }
}
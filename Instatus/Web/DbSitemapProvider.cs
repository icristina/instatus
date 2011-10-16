﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
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
            using (var db = BaseDataContext.Instance())
            {
                var nodes = db.GetPage<Page>(node.Key, new string[] { "Related" })
                                .Related
                                .Select(p => p.ToSiteMapNode(this))
                                .ToArray();

                return new SiteMapNodeCollection(nodes);
            }
        }

        public override SiteMapNode GetParentNode(SiteMapNode node)
        {
            throw new NotImplementedException();
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            using (var db = BaseDataContext.Instance())
            {
                return db.Applications.First().ToSiteMapNode(this);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using Instatus.Models;

namespace Instatus.Web
{
    public class DbSitemapProvider : SiteMapProvider
    {
        public static Func<Page, bool> IsNavigable = p =>
        {
            return p.Priority >= 0;
        };
        
        public override SiteMapNode FindSiteMapNode(string rawUrl)
        {
            throw new NotImplementedException();
        }

        public override SiteMapNodeCollection GetChildNodes(SiteMapNode node)
        {
            var pageContext = WebApp.GetService<IPageContext>();

            var set = new WebSet()
            {
                Expand = new string[] { "Pages" },
                Kind = WebKind.Article
            };

            var nodes = pageContext.GetPage(node.Key, set)
                            .Pages
                            .OfType<Article>()
                            .Where(IsNavigable)
                            .Select(p => p.ToSiteMapNode(this))
                            .ToArray();

            pageContext.TryDispose();

            return new SiteMapNodeCollection(nodes);
        }

        public override SiteMapNode GetParentNode(SiteMapNode node)
        {
            throw new NotImplementedException();
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            using (var db = WebApp.GetService<IApplicationContext>())
            {
                return db.Pages.OfType<Application>().First().ToSiteMapNode(this);
            }
        }
    }
}
using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Scaffold.Entities;
using Instatus.Integration.Mvc;
using Instatus.Core.Extensions;

namespace Instatus.Sample.Controllers
{
    public class BlogController : Controller
    {
        private IEntityStorage entityStorage;
        
        public ActionResult Index(int pageIndex = 0, int pageSize = 10)
        {
            var posts = entityStorage
                .Set<Post>()
                .OrderByDescending(p => p.Created);
            
            ViewData.Model = new PagedViewModel<Post>(posts, pageIndex, pageSize);
            
            return View();
        }

        public BlogController(IEntityStorage entityStorage) 
        {
            this.entityStorage = entityStorage;
        }
    }
}
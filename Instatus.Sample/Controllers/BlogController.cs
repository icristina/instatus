using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Scaffold.Entities;
using Instatus.Integration.Mvc;
using Instatus.Core.Extensions;
using System.Collections;
using Instatus.Scaffold.Models;

namespace Instatus.Sample.Controllers
{
    public class BlogController : Instatus.Scaffold.Controllers.BlogController
    {
        public override ActionResult Index(int pageIndex = 0, string tag = null)
        {
            ViewData.Model = new Blog(GetPosts(tag), pageIndex, 10, 9)
            {
                TagCloud = new TagCloud(GetTags())
            };
            
            return View();
        }
        
        public BlogController(IEntityStorage entityStorage) 
            : base(entityStorage)
        {

        }
    }
}
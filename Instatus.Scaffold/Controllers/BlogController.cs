using Instatus.Core;
using Instatus.Integration.Mvc;
using Instatus.Scaffold.Entities;
using Instatus.Scaffold.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Instatus.Scaffold.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class BlogController : Controller
    {
        private IEntityStorage entityStorage;
        
        private Expression<Func<Post, BlogPost>> selectBlogPost = (p) => new BlogPost() 
        {
            Title = p.Name,
            Body = p.Content,
            Tags = p.Tags.Select(t => t.Name),
            Slug = p.Slug,
            Picture = p.Picture,
            Published = p.Created
        };

        public ActionResult Index(int pageIndex = 0, int pageSize = 10, string tag = null)
        {
            IQueryable<Post> posts = entityStorage
                .Set<Post>()
                .Where(p => p.State == State.Approved && p.Category == WellKnown.Kind.BlogPost);

            if (tag != null)
            {
                posts = posts.Where(p => p.Tags.Any(t => t.Name == tag));
            }

            var orderedPosts = posts
                .Select(selectBlogPost)
                .OrderByDescending(b => b.Published);

            var tags = entityStorage
                .Set<Tag>()
                .Where(t => t.Posts.Any(p => p.State == State.Approved && p.Category == WellKnown.Kind.BlogPost))
                .OrderBy(t => t.Name)
                .Select(t => new 
                {
                    Name = t.Name,
                    Count = t.Posts.Count()
                })
                .ToList()
                .ToDictionary(t => t.Name, t => t.Count);

            ViewData.Model = new Blog(orderedPosts, pageIndex, pageSize)
            {
                TagCloud = new TagCloud(tags)
            };
            
            return View();
        }

        [HttpNotFound]
        public ActionResult Details(string slug)
        {
            ViewData.Model = entityStorage
                .Set<Post>()
                .Where(p => p.Slug == slug)
                .Select(selectBlogPost)
                .FirstOrDefault();
            
            return View();
        }

        public BlogController(IEntityStorage entityStorage)
        {
            this.entityStorage = entityStorage;
        }
    }
}

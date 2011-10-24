﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Controllers;
using Instatus.Data;
using System.Web.Security;
using Instatus.Web;
using System.Net;
using Instatus.Models;

namespace Instatus.Areas.Wordpress.Controllers
{
    public class WordpressApiController : BaseController<BaseDataContext>
    {
        private void Authenticate(string username, string password)
        {
            if (!Membership.ValidateUser(username, password))
                throw new HttpException((int)HttpStatusCode.Forbidden, "Invalid username or password");
        }

        [ActionName("system.listMethods")]
        public ActionResult System_ListMethods()
        {
            ViewData.Model = new string[] {
                "blogger.getUsersBlogs",
                "wp.getUsersBlogs",
                "wp.getPages",
                "wp.getCategories",
                "metaWeblog.getRecentPosts",
                "metaWeblog.editPost",
                "metaWeblog.newPost",
                "metaWeblog.getPost",
                "wp.editPage",
                "wp.editPost",
                "wp.getComments",
                "mt.setPostCategories"
            };
            
            return new XmlRpcResult();
        }

        [ActionName("blogger.getUsersBlogs")]
        public ActionResult Blogger_GetUsersBlogs(string appKey, string username, string password)
        {
            Authenticate(username, password);
            
            ViewData.Model = Context
                .Applications
                .ToList()
                .Select(a => new BlogInfo()
                {
                    Blogid = a.Slug,
                    BlogName = a.Name,
                    IsAdmin = true,
                    Url = WebPaths.BaseUri.ToString(),
                    Xmlrpc = WebPaths.BaseUri.ToString()
                })
                .ToArray();

            return new XmlRpcResult();
        }

        [ActionName("wp.getUsersBlogs")]
        public ActionResult Wordpress_GetUsersBlogs(string username, string password)
        {
            return Blogger_GetUsersBlogs(null, username, password);
        }

        [ActionName("wp.getPages")]
        public ActionResult GetPages(int blogId, string username, string password, int maxPages)
        {
            Authenticate(username, password);

            ViewData.Model = Context
                .Pages
                .OfType<Article>()
                .Select(p => new WordpressPage()
                {
                    Page_id = p.Id,
                    Title = p.Name
                }).ToArray();

            return new XmlRpcResult();
        }

        [ActionName("wp.getCategories")]
        public ActionResult GetCategories(int blogId, string username, string password)
        {
            Authenticate(username, password);

            ViewData.Model = Context
                .Tags
                .Select(t => new WordpressCategory()
                {
                    CategoryId = t.Id,
                    CategoryName = t.Name
                }).ToArray();

            return new XmlRpcResult();
        }

        [ActionName("metaWeblog.getRecentPosts")]
        public ActionResult GetRecentPosts(int blogid, string username, string password, int numberOfPosts)
        {
            Authenticate(username, password);

            ViewData.Model = Context
                .Pages
                .OfType<Post>()
                .ToList()
                .Select(p => new MetaWeblogPost()
                {
                    Postid = p.Id,
                    Title = p.Name,
                    DateCreated = p.CreatedTime,
                    Description = p.Document.Body
                })
                .ToArray();

            return new XmlRpcResult();
        }

        [ActionName("metaWeblog.editPost")]
        public ActionResult EditPost(int postid, string username, string password, MetaWeblogPost metaWeblogPost)
        {
            Authenticate(username, password);

            var post = Context.Pages.Find(postid);

            post.Slug = metaWeblogPost.Title.ToSlug();
            post.Name = metaWeblogPost.Title;
            post.Description = metaWeblogPost.Description;
            post.Status = metaWeblogPost.Post_status == "publish" ? WebStatus.Published.ToString() : WebStatus.Draft.ToString();

            Context.SaveChanges();

            return new XmlRpcResult(true);
        }

        [ActionName("metaWeblog.newPost")]
        public ActionResult NewPost(int blogid, string username, string password, MetaWeblogPost metaWeblogPost, bool publish)
        {
            Authenticate(username, password);

            var post = new Post()
            {
                Slug = metaWeblogPost.Title.ToSlug(),
                Name = metaWeblogPost.Title,
                Description = metaWeblogPost.Description,
                CreatedTime = metaWeblogPost.DateCreated.HasValue ? metaWeblogPost.DateCreated.Value : DateTime.Now,
                Status = publish ? WebStatus.Published.ToString() : WebStatus.Draft.ToString()
            };

            Context.Pages.Add(post);
            Context.SaveChanges();

            return new XmlRpcResult(post.Id);
        }

        [ActionName("metaWeblog.getPost")]
        public ActionResult GetPost(int postid, string username, string password)
        {
            Authenticate(username, password);

            var post = Context.Pages.Find(postid);

            ViewData.Model = new MetaWeblogPost()
            {
                Postid = post.Id,
                Title = post.Name,
                Description = post.Description,
                DateCreated = post.CreatedTime
            };

            return new XmlRpcResult();
        }

        [ActionName("wp.editPage")]
        public ActionResult EditPage(int blogid, int pageid, string username, string password, WordpressPage wordpressPage)
        {
            Authenticate(username, password);

            var page = Context.Pages.Find(pageid);

            page.Slug = wordpressPage.Title.ToSlug();
            page.Name = wordpressPage.Title;
            page.Description = wordpressPage.Description;
            page.Status = wordpressPage.Page_status == "publish" ? WebStatus.Published.ToString() : WebStatus.Draft.ToString();

            Context.SaveChanges();

            return new XmlRpcResult(true);
        }

        [ActionName("wp.getComments")]
        public ActionResult GetComments(int blogid, string username, string password, WordpressCommentQuery commentQuery)
        {
            Authenticate(username, password);

            ViewData.Model = Context.Messages
                .OfType<Comment>()
                .ToList()
                .Select(c => new WordpressComment()
                {
                    Post_id = "0",
                    Content = c.Body
                })
                .ToArray();

            return new XmlRpcResult();
        }

        [ActionName("mt.setPostCategories")]
        public ActionResult SetPostCategories(int postid, string username, string password, MetaWeblogCategory[] categories)
        {
            Authenticate(username, password);

            var post = Context.Pages.Find(postid);

            if (post == null)
                return new XmlRpcResult(false);

            var tagIds = categories.Select(c => c.CategoryId.AsInteger());

            post.Tags = BindingHelpers.UpdateList<Tag, int>(Context.Tags, post.Tags, tagIds);

            Context.SaveChanges();

            return new XmlRpcResult(true);
        }
    }

    // Wordpress
    public class WordpressTemplate
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class WordpressOption
    {
        public string Option { get; set; }
        public string Value { get; set; }
    }

    public class WordpressFile
    {
        public string File { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }

    public class WordpressFileRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Bits { get; set; }
        public bool Overwrite { get; set; }
    }

    public class WordpressCategoryRequest
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Parent_id { get; set; }
        public string Description { get; set; }
    }

    public class WordpressCategory
    {
        public int CategoryId { get; set; }
        public int ParentId { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string HtmlUrl { get; set; }
        public string RssUrl { get; set; }
    }

    public class WordpressCommentQuery
    {
        public string Post_id { get; set; }
        public string Status { get; set; }
        public int Offset { get; set; }
        public int Number { get; set; }
    }

    public class WordpressComment
    {
        public DateTime DateCreated { get; set; }
        public string User_id { get; set; }
        public string Comment_id { get; set; }
        public string Parent { get; set; }
        public string Status { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public string Post_id { get; set; }
        public string Post_title { get; set; }
        public string Author { get; set; }
        public string Author_url { get; set; }
        public string Author_email { get; set; }
        public string Author_ip { get; set; }

        public DateTime? Date_created_gmt { get; set; }
        public int Comment_parent { get; set; }
    }

    public class WordpressUser
    {
        public int User_id { get; set; }
        public string User_login { get; set; }
        public string Display_name { get; set; }
        public string User_email { get; set; }
        public string Meta_value { get; set; }
    }

    public class WordpressPage
    {
        public string Wp_slug { get; set; }
        public string Wp_password { get; set; }
        public int Wp_page_parent_id { get; set; }
        public int Wp_page_order { get; set; }
        public int Wp_author_id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Mt_excerpt { get; set; }
        public string Mt_text_more { get; set; }
        public int Mt_allow_comments { get; set; }
        public int Mt_allow_pings { get; set; }
        public DateTime? DateCreated { get; set; }
        public WordpressPageField[] Custom_fields { get; set; }

        // list
        public int Userid { get; set; }
        public int Page_id { get; set; }
        public string Page_title { get; set; }
        public string Page_status { get; set; }
        public string Link { get; set; }
        public string PermaLink { get; set; }
        public string[] Categories { get; set; }
        public string Excerpt { get; set; }
        public string Text_more { get; set; }
        public string Wp_author { get; set; }
        public string Wp_page_parent_title { get; set; }
        public string Wp_author_display_name { get; set; }
        public DateTime? Date_created_gmt { get; set; }
        public string Wp_page_template { get; set; }
    }

    public class WordpressPageField
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    // Blogger
    public class BlogInfo
    {
        public bool IsAdmin { get; set; }
        public string Blogid { get; set; }
        public string BlogName { get; set; }
        public string Url { get; set; }
        public string Xmlrpc { get; set; }
    }

    // MetaWeblog
    public class MetaWeblogPost
    {
        public int Postid { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Post_status { get; set; }
    }

    public class MetaWeblogCategory
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Title { get; set; }
    }
}

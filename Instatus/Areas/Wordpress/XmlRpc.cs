﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Runtime.Serialization;
using Instatus.Models;
using Instatus.Web;
using System.Web.Security;

namespace Instatus.Areas.Wordpress
{
    // %USERPROFILE%\Local Settings\Application Data\Windows Live Writer\
    [ServiceContract]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BlogService
    {
        [OperationContract(Action = "blogger.getUsersBlogs")]
        public BlogInfo[] GetUsersBlogs(string appKey, string username, string password)
        {
            if (!Membership.ValidateUser(username, password)) return null;
            
            using(var db = BaseDataContext.Instance()) {
                return db.Applications
                    .ToList()
                    .Select(a => new BlogInfo()
                    {
                        blogid = a.Slug,
                        blogName = a.Name,
                        isAdmin = true,
                        url = Paths.BaseUri.ToString(),
                        xmlrpc = Paths.BaseUri.ToString() + "/weblog"
                    })
                    .ToArray();
            }
        }

        //[XmlRpcMethod("wp.getComments")]
        public WordpressComment[] GetComments(int blogId, string username, string password, WordpressCommentQuery query)
        {
            return null;
        }

        [OperationContract(Action = "wp.getCategories")]
        public WordpressCategory[] GetCategories(string blogId, string username, string password)
        {
            if (!Membership.ValidateUser(username, password)) return null;
            
            using (var db = BaseDataContext.Instance())
            {
                return db.Tags.Select(t => new WordpressCategory()
                {
                    categoryId = t.Id,
                    categoryName = t.Name
                }).ToArray();
            }
        }

        //[XmlRpcMethod("wp.newCategory")]
        public int NewCategory(string blogid, string username, string password, WordpressCategoryRequest category)
        {
            return 0;
        }

        //[XmlRpcMethod("blogger.deletePost")]
        public bool DeletePost(string appKey, string postid, string username, string password, bool publish)
        {
            return true;
        }

        //[XmlRpcMethod("metaWeblog.getCategories")]
        //public MetaWeblogCategory[] GetCategories(string blogid, string username, string password)
        //{
        //    return null;
        //}

        [OperationContract(Action = "metaWeblog.newPost")]
        public string NewPost(string blogid, string username, string password, MetaWeblogPost metaWeblogPost, bool publish)
        {
            if (!Membership.ValidateUser(username, password)) return null;
            
            using (var db = BaseDataContext.Instance())
            {
                var post = new Post()
                {
                    Slug = metaWeblogPost.title.ToSlug(),
                    Name = metaWeblogPost.title,
                    Description = metaWeblogPost.description,
                    CreatedTime = metaWeblogPost.dateCreated.HasValue ? metaWeblogPost.dateCreated.Value : DateTime.Now,
                    Status = publish ? WebStatus.Published.ToString() : WebStatus.Draft.ToString()
                };
                db.Pages.Add(post);
                db.SaveChanges();
                return post.Id.ToString();
            }
        }

        [OperationContract(Action = "metaWeblog.getPost")]
        public MetaWeblogPost GetPost(string postid, string username, string password)
        {
            if (!Membership.ValidateUser(username, password)) return null;
            
            using (var db = BaseDataContext.Instance())
            {
                var id = postid.AsInteger();
                var post = db.Pages.Find(id);

                return new MetaWeblogPost()
                {
                    postid = post.Id.ToString(),
                    title = post.Name,
                    description = post.Description,
                    dateCreated = post.CreatedTime
                };
            }
        }

        //[XmlRpcMethod("metaWeblog.getRecentPosts")]
        public MetaWeblogPost[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            return null;
        }

        [OperationContract(Action = "metaWeblog.editPost")]
        public bool EditPost(string postid, string username, string password, MetaWeblogPost metaWeblogPost, bool publish)
        {
            if (!Membership.ValidateUser(username, password)) return false;

            using (var db = BaseDataContext.Instance())
            {
                var id = postid.AsInteger();
                var post = db.Pages.Find(id);

                post.Slug = metaWeblogPost.title.ToSlug();
                post.Name = metaWeblogPost.title;
                post.Description = metaWeblogPost.description;
                post.Status = publish ? WebStatus.Published.ToString() : WebStatus.Draft.ToString();

                db.SaveChanges();
            }

            return true;
        }

        [OperationContract(Action = "mt.setPostCategories")]
        public bool SetPostCategories(string postid, string username, string password, MetaWeblogCategory[] categories)
        {
            if (!Membership.ValidateUser(username, password)) return false;

            using (var db = BaseDataContext.Instance())
            {
                var id = postid.AsInteger();
                var post = db.Pages.Find(id);

                if (post == null) return false;

                var tagIds = categories.Select(c => c.categoryId.AsInteger());

                post.Tags = BindingHelpers.UpdateList<Tag, int>(db.Tags, post.Tags, tagIds);

                db.SaveChanges();
            }

            return true;
        }
    }

    // Wordpress
    public class WordpressTemplate
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class WordpressOption
    {
        public string option { get; set; }
        public string value { get; set; }
    }

    public class WordpressFile
    {
        public string file { get; set; }
        public string url { get; set; }
        public string type { get; set; }
    }

    public class WordpressFileRequest
    {
        public string name { get; set; }
        public string type { get; set; }
        public string bits { get; set; }
        public bool overwrite { get; set; }
    }

    public class WordpressCategoryRequest
    {
        public string name { get; set; }
        public string slug { get; set; }
        public int parent_id { get; set; }
        public string description { get; set; }
    }

    public class WordpressCategory
    {
        public int categoryId { get; set; }
        public int parentId { get; set; }
        public string description { get; set; }
        public string categoryName { get; set; }
        public string htmlUrl { get; set; }
        public string rssUrl { get; set; }
    }

    public class WordpressCommentQuery
    {
        public string post_id { get; set; }
        public string status { get; set; }
        public int offset { get; set; }
        public int number { get; set; }
    }

    public class WordpressComment
    {
        public DateTime dateCreated { get; set; }
        public string user_id { get; set; }
        public string comment_id { get; set; }
        public string parent { get; set; }
        public string status { get; set; }
        public string content { get; set; }
        public string link { get; set; }
        public string post_id { get; set; }
        public string post_title { get; set; }
        public string author { get; set; }
        public string author_url { get; set; }
        public string author_email { get; set; }
        public string author_ip { get; set; }

        public DateTime? date_created_gmt { get; set; }
        public int comment_parent { get; set; }
    }

    public class WordpressUser
    {
        public int user_id { get; set; }
        public string user_login { get; set; }
        public string display_name { get; set; }
        public string user_email { get; set; }
        public string meta_value { get; set; }
    }

    public class WordpressPage
    {
        public string wp_slug { get; set; }
        public string wp_password { get; set; }
        public int wp_page_parent_id { get; set; }
        public int wp_page_order { get; set; }
        public int wp_author_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string mt_excerpt { get; set; }
        public string mt_text_more { get; set; }
        public int mt_allow_comments { get; set; }
        public int mt_allow_pings { get; set; }
        public DateTime? dateCreated { get; set; }
        public WordpressPageField[] custom_fields { get; set; }

        // list
        public int userid { get; set; }
        public int page_id { get; set; }
        public string page_status { get; set; }
        public string link { get; set; }
        public string permaLink { get; set; }
        public string[] categories { get; set; }
        public string excerpt { get; set; }
        public string text_more { get; set; }
        public string wp_author { get; set; }
        public string wp_page_parent_title { get; set; }
        public string wp_author_display_name { get; set; }
        public DateTime? date_created_gmt { get; set; }
        public string wp_page_template { get; set; }
    }

    public class WordpressPageField
    {
        public int id { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }

    // Blogger
    [DataContract]
    public class BlogInfo
    {
        [DataMember] public bool isAdmin { get; set; }
        [DataMember] public string blogid { get; set; }
        [DataMember] public string blogName { get; set; }
        [DataMember] public string url { get; set; }
        [DataMember] public string xmlrpc { get; set; }
    }

    // MetaWeblog
    public class MetaWeblogPost
    {
        public string postid { get; set; }
        public DateTime? dateCreated { get; set; }
        public string description { get; set; }
        public string title { get; set; }
    }

    public class MetaWeblogCategory
    {
        public string categoryId { get; set; }
        public string categoryName { get; set; }
        public string title { get; set; }
    }
}
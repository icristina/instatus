using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using Instatus.Web;
using Instatus.Models;

namespace Instatus
{
    public static class SocialQueries
    {
        public static void SetStatus<T>(this BaseDataContext context, int id, WebStatus status) where T : class, IUserGeneratedContent
        {
            context.Set<T>().Find(id).Status = status.ToString();
        }

        public static void MarkAsSpam<T>(this BaseDataContext context, int id) where T : class, IUserGeneratedContent
        {
            context.SetStatus<T>(id, WebStatus.Spam);
        }

        public static void MarkAsPublished<T>(this BaseDataContext context, int id) where T : class, IUserGeneratedContent
        {
            context.SetStatus<T>(id, WebStatus.Published);
        }

        public static RecordResult<Post> Post(this BaseDataContext context, string message)
        {
            return context.Post(new WebEntry()
            {
                Description = message
            });
        }

        public static RecordResult<Post> Post(this BaseDataContext context, WebEntry entry)
        {
            var user = context.GetCurrentUser();

            if (!user.Can(WebVerb.Post))
                return RecordResult<Post>.Failed;

            var post = new Post()
            {
                Description = entry.Description,
                User = user
            };

            context.Pages.Add(post);

            return new RecordResult<Post>(post);
        }

        public static RecordResult<Activity> Like<T>(this BaseDataContext context, int id) where T : class, IUserGeneratedContent
        {
            var user = context.GetCurrentUser();

            if (!user.Can(WebVerb.Like))
                return RecordResult<Activity>.Failed;

            var userId = user.Id;
            var like = WebVerb.Like.ToString();
            var content = context.Set<T>().Find(id);

            // Failure:
            // Like own content
            // Duplicate Like
            if (content.User.Id == userId || content.Activities.Any(a => a.Verb == like && a.UserId == userId))
                return RecordResult<Activity>.Failed;

            var activity = new Activity()
            {
                Verb = like,
                User = user
            };

            content.Activities.Add(activity);

            return new RecordResult<Activity>(activity);
        }

        public static RecordResult<Comment> Comment<T>(this BaseDataContext context, int id, string body) where T : class, IUserGeneratedContent
        {
            var user = context.GetCurrentUser();

            if (!user.Can(WebVerb.Comment))
                return RecordResult<Comment>.Failed;

            var content = context.Set<T>().Find(id);

            var comment = new Comment()
            {
                PageId = id,
                User = user,
                Body = body
            };

            context.Messages.Add(comment);

            return new RecordResult<Comment>(comment);
        }
    }
}
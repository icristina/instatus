using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Data;
using Instatus.Web;
using Instatus.Models;
using System.Web.Routing;
using System.Collections.Specialized;
using System.Data;

namespace Instatus.Commands
{
    public class AwardCommand : IWebCommand
    {
        private string achievementSlug;
        
        public string Name
        {
            get
            {
                return "Award";
            }
        }

        public WebLink GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            User user = User.From(viewModel); 

            using (var db = BaseDataContext.Instance())
            {
                var achievement = db.GetPage<Achievement>(achievementSlug);
                
                if (!db.Activities.OfType<Award>().Any(a => a.UserId == user.Id && a.AchievementId == achievement.Id))
                {
                    return new WebLink()
                    {
                        Title = WebPhrase.Flag(achievement.Name),
                        Uri = urlHelper.Action("Command", new { id = viewModel.Id, commandName = Name, commandValue = true })
                    };
                }
                else
                {
                    return new WebLink()
                    {
                        Title = WebPhrase.RemoveFlag(achievement.Name),
                        Uri = urlHelper.Action("Command", new { id = viewModel.Id, commandName = Name, commandValue = false })
                    };
                }                
            }
        }

        public bool Execute(dynamic viewModel, UrlHelper urlHelper, RouteData routeData, NameValueCollection requestParams)
        {
            User userModel = User.From(viewModel);
            var userId = userModel != null ? userModel.Id : routeData.Id();
            var awarded = requestParams.Value<bool>("commandValue");
            
            using (var db = BaseDataContext.Instance())
            {
                var user = db.Users.Find(userId);
                
                if(awarded) {
                    var achievement = db.GetPage<Achievement>(achievementSlug);

                    if (user.Activities == null)
                        user.Activities = new List<Activity>();

                    var award = new Award()
                    {
                        Achievement = achievement
                    };

                    if (viewModel is Page)
                    {
                        award.PageId = viewModel.Id;
                    }
                    else if (viewModel is Activity)
                    {
                        var parentActivity = db.Activities.Find(viewModel.Id);

                        db.Activities.Attach(parentActivity);
                        
                        if (parentActivity.Activities == null)
                            parentActivity.Activities = new List<Activity>();

                        parentActivity.Activities.Add(award);
                    }

                    user.Activities.Add(award);

                    db.LogChange(user, "awarded " + achievement.Name);
                } else {
                    var award = db
                        .Activities
                        .OfType<Award>()
                        .Where(a => a.Achievement.Slug == achievementSlug)
                        .OrderByDescending(a => a.CreatedTime)
                        .FirstOrDefault();

                    db.LogChange(user, "removed award " + award.Achievement.Name);

                    foreach (var activity in award.Parents.ToList())
                    {
                        award.Parents.Remove(activity);
                    }

                    db.MarkDeleted(award);
                }
                
                db.SaveChanges();
            }

            return true;
        }

        public AwardCommand(string achievementSlug) {
            this.achievementSlug = achievementSlug;
        }
    }
}

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
            var award = WebVerb.Award.ToString();

            using (var db = BaseDataContext.Instance())
            {
                if (!db.Activities.Any(a => a.UserId == user.Id && a.Verb == award && a.Parent.Slug == achievementSlug))
                {
                    return new WebLink()
                    {
                        Title = "Award",
                        Uri = urlHelper.Action("Command", new { id = viewModel.Id, commandName = Name, commandValue = true })
                    };
                }
                else
                {
                    return new WebLink()
                    {
                        Title = "Remove award",
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

                    user.Activities.Add(new Award() {
                        Parent = achievement
                    });
                } else {
                    var award = db
                        .Activities
                        .OfType<Award>()
                        .Where(a => a.Parent.Slug == achievementSlug)
                        .OrderByDescending(a => a.CreatedTime)
                        .FirstOrDefault();

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

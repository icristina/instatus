using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using Instatus.Web;
using System.ComponentModel.Composition;
using System.Collections;
using Instatus.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Instatus.Export
{
    [Export(typeof(IDataExport))]
    public class SubscriberExport : IDataExport
    {
        public object DefaultConfiguration
        {
            get
            {
                return new SubscriberExportConfiguration();               
            }
        }
        
        public IEnumerable Export(object configuration)
        {
            using (var db = WebApp.GetService<IApplicationContext>())
            {
                var subscriberExportConfiguration = configuration as SubscriberExportConfiguration;
                var users = db.Users.AsQueryable();

                if (subscriberExportConfiguration.Subscription.HasValue)
                {
                    users = users.Where(u => u.Subscriptions.Any(s => s.Id == subscriberExportConfiguration.Subscription));
                }
                else
                {
                    users = users.Where(u => u.Subscriptions.Any());
                }
                
                return users
                        .OrderBy(u => u.CreatedTime)
                        .Select(u => new
                        {
                            GivenName = u.Name.GivenName,
                            FamilyName = u.Name.FamilyName,
                            EmailAddress = u.EmailAddress,
                            CreatedTime = u.CreatedTime
                        })
                        .ToList();
            }
        }

        public string Name
        {
            get
            {
                return "Subscribers";
            }
        }
    }

    public class SubscriberExportConfiguration : IDataboundModel
    {
        [Column("Subscription")]
        [Display(Name = "Subscription")]
        public SelectList SubscriptionList { get; set; }

        [ScaffoldColumn(false)]
        public int? Subscription { get; set; }

        public void Databind()
        {
            using (var db = WebApp.GetService<IApplicationContext>())
            {
                SubscriptionList = new SelectList(db.Subscriptions.ToList(), "Id", "Name");
            }  
        }
    }
}
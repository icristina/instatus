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
using System.ComponentModel.DataAnnotations.Schema;
using Instatus.Entities;

namespace Instatus.Export
{
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
            var applicationModel = DependencyResolver.Current.GetService<IApplicationModel>();
            var subscriberExportConfiguration = configuration as SubscriberExportConfiguration;
            var users = applicationModel.Users.AsQueryable();

            if (subscriberExportConfiguration.Subscription.HasValue)
            {
                users = users.Where(u => u.Subscriptions.Any(s => s.Page.Id == subscriberExportConfiguration.Subscription));
            }
            else
            {
                users = users.Where(u => u.Subscriptions.Any());
            }
                
            return users
                    .OrderBy(u => u.CreatedTime)
                    .Select(u => new
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        EmailAddress = u.EmailAddress,
                        CreatedTime = u.CreatedTime
                    })
                    .ToList();
        }

        public string Name
        {
            get
            {
                return "Subscribers";
            }
            set
            {
                throw new NotImplementedException();
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
            var applicationModel = DependencyResolver.Current.GetService<IApplicationModel>();
            var campaigns = applicationModel.Pages.Where(p => p.Kind == "Campaign").ToList();

            SubscriptionList = new SelectList(campaigns, "Id", "Name");
        }
    }
}
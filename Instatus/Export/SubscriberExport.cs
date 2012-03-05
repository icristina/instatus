using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using Instatus.Web;
using System.ComponentModel.Composition;
using System.Collections;
using Instatus.Models;

namespace Instatus.Export
{
    [Export(typeof(IDataExport))]
    public class SubscriberExport : IDataExport
    {
        public IEnumerable Data
        {
            get
            {
                using (var db = WebApp.GetService<IApplicationContext>())
                {
                    return db.Users
                            .Where(u => u.Subscriptions.Any())
                            .OrderBy(u => u.CreatedTime)
                            .Select(u => new SubscriberData()
                            {
                                GivenName = u.Name.GivenName,
                                FamilyName = u.Name.FamilyName,
                                EmailAddress = u.EmailAddress,
                                CreatedTime = u.CreatedTime
                            })
                            .ToList();
                }
            }
        }

        public string Name
        {
            get
            {
                return "Subscribers";
            }
        }

        private class SubscriberData
        {
            public string GivenName { get; set; }
            public string FamilyName { get; set; }
            public string EmailAddress { get; set; }
            public DateTime CreatedTime { get; set; }
        }
    }
}
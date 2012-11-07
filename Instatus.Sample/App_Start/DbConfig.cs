using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Instatus.Core;
using Instatus.Integration.Server;
using Instatus.Core.Impl;
using Instatus.Integration.Mvc;
using System.Data.Entity;
using Instatus.Scaffold.Entities;

namespace Instatus.Sample
{
    public class DbConfig
    {
        public static void RegisterProviders()
        {
            Database.SetInitializer<InstatusSamplelDb>(new SocialDbInitializer());
        }
    }

    public class InstatusSamplelDb : SocialDb
    {
        public InstatusSamplelDb()
            : base("InstatusSample")
        {

        }
    }

    public class SocialDbInitializer : DropCreateDatabaseAlways<SocialDb>
    {
        protected override void Seed(SocialDb context)
        {
            context.Posts.Add(new Post()
            {
                Locale = WellKnown.Locale.UnitedStates,
                Slug = "home",
                Name = "Home",
                Content = "<p>Homepage</p>"
            });
            
            base.Seed(context);
        }
    }
}
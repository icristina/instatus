using Instatus.Server;
using InstatusSample.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InstatusSample.Controllers
{
    public class AccountController : GoogleAccountController<User>
    {
        private UserManager<User> userManager;

        public override UserManager<User> UserManager
        {
            get
            {
                LazyInitializer.EnsureInitialized(ref userManager, () => new UserManager<User>(new EntityUserStore<InstatusSampleDb, User>()));

                return userManager;
            }
            set
            {
                userManager = value;
            }
        }
    }
}
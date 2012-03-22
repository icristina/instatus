using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Services
{
    [Export(typeof(IMembershipService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]    
    public class DbMembershipService : IMembershipService
    {
        private IApplicationContext applicationContext;
        
        public bool ValidateUser(string username, string password)
        {
            using (applicationContext)
            {
                if (username.IsEmpty() || password.IsEmpty())
                    return false;

                var user = applicationContext.GetUser(username);

                return user != null && user.Password == password.ToEncrypted();
            }
        }

        public string[] GetRolesForUser(string username)
        {
            using (applicationContext)
            {
                var user = applicationContext.GetUser(username);

                if (user == null) return null;

                return user.Roles
                        .Select(r => r.Name)
                        .ToArray();
            }
        }

        [ImportingConstructor]
        public DbMembershipService(IApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }
    }
}
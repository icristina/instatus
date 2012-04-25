using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Services;

namespace Instatus.Entities
{
    public class DbMembershipService : IMembershipService
    {
        private IApplicationModel applicationModel;
        
        public bool ValidateUser(string username, string password)
        {
            if (username.IsEmpty() || password.IsEmpty())
                return false;

            var user = applicationModel.Users.Where(u => u.EmailAddress == username).FirstOrDefault();

            return user != null && user.Password == password.ToEncrypted();
        }

        public string[] GetRolesForUser(string username)
        {
            var user = applicationModel.Users.Where(u => u.EmailAddress == username).FirstOrDefault();

            if (user == null) return null;

            return user.Role
                    .ToStringList()
                    .ToArray();
        }

        public bool ValidateVerificationToken(int userId, string token)
        {
            var user = applicationModel.Users.Find(userId);

            if (user.Verified)
                return true;
            
            var correctVerificationToken = GenerateVerificationToken(user);

            if (token.Match(correctVerificationToken))
            {
                user.Verified = true;
                applicationModel.SaveChanges();
            }

            return false;
        }

        private static string GenerateVerificationToken(User user) 
        {
            return user.Password.Substring(0, 10);;
        }

        public DbMembershipService(IApplicationModel applicationModel)
        {
            this.applicationModel = applicationModel;
        }
    }
}
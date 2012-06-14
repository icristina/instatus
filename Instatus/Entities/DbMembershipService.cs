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
        
        public bool ValidatePassword(string username, string password)
        {
            if (username.IsEmpty() || password.IsEmpty())
                return false;

            var user = applicationModel.Users.Where(FilterBy.UserName(username)).FirstOrDefault();

            return user != null && user.Password == password.ToEncrypted();
        }

        public string[] GetRolesForUser(string username)
        {
            var user = applicationModel.Users.Where(FilterBy.UserName(username)).FirstOrDefault();

            if (user == null) return null;

            return user.Role
                    .ToList()
                    .ToArray();
        }

        public bool ValidateToken(string username, string token)
        {
            return GenerateToken(username).Match(token);
        }

        public bool VerifyUser(string username, string token)
        {
            var user = applicationModel.Users.Where(FilterBy.UserName(username)).FirstOrDefault();

            if (user.Verified)
                return true;

            if (!ValidateToken(username, token))
                return false;

            user.Verified = true;
            applicationModel.SaveChanges();

            return true;
        }
        
        public string GenerateToken(string username)
        {
            var user = applicationModel.Users.Where(FilterBy.UserName(username)).FirstOrDefault();
            return user.Password.Substring(0, 10);
        }
        
        public void ChangePassword(string username, string password)
        {
            var user = applicationModel.Users.Where(FilterBy.UserName(username)).FirstOrDefault();
            user.Password = password;
            applicationModel.SaveChanges();
        }        
        
        public DbMembershipService(IApplicationModel applicationModel)
        {
            this.applicationModel = applicationModel;
        }
    }
}
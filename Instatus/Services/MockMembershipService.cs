using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using System.ComponentModel.Composition;

namespace Instatus.Services
{
    public class MockMembershipService : IMembershipService
    {
        public static string[] AllRoles = new string[] {
            WebConstant.Role.Editor,
            WebConstant.Role.Moderator,
            WebConstant.Role.Developer
        };
        
        public bool ValidatePassword(string username, string password)
        {
            return true;
        }

        public string[] GetRolesForUser(string username)
        {
            return AllRoles;
        }
        
        public string GenerateToken(string username)
        {
            return string.Empty;
        }
        
        public bool ValidateToken(string username, string token)
        {
            return true;
        }

        public void ChangePassword(string username, string password)
        {
            // do nothing
        }

        public bool VerifyUser(string username, string token)
        {
            return true;
        }
    }
}
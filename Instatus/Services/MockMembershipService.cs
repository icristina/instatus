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
        
        public bool ValidateUser(string username, string password)
        {
            return true;
        }

        public string[] GetRolesForUser(string username)
        {
            return AllRoles;
        }

        public bool ValidateVerificationToken(int userId, string token)
        {
            return true;
        }
    }
}
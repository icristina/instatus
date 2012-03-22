using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using System.ComponentModel.Composition;

namespace Instatus.Services
{
    [Export(typeof(IMembershipService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]    
    public class MockMembershipService : IMembershipService
    {
        public bool ValidateUser(string username, string password)
        {
            return true;
        }

        public string[] GetRolesForUser(string username)
        {
            return new WebRole[] {
                WebRole.Visitor,
                WebRole.Member,
                WebRole.Tester,
                WebRole.Executive,
                WebRole.Moderator,
                WebRole.Author,
                WebRole.Editor,
                WebRole.Administrator,
                WebRole.Developer
            }
            .ToStringList()
            .ToArray();
        }
    }
}
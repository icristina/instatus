using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class MockMembership : IMembership
    {
        public bool ValidateUser(string userName, string password)
        {
            return true;
        }

        public string[] GetRoles(string userName)
        {
            return new string[] { "Administrator", "Moderator", "Member", "User" };
        }

        public string GenerateVerificationToken(string userName)
        {
            return Guid.NewGuid().ToString();
        }

        public bool ValidateVerificationToken(string userName, string verificationToken)
        {
            return true;
        }

        public bool ChangePassword(string userName, string verificationToken, string newPassword)
        {
            return true;
        }

        public bool ValidateExternalUser(string providerName, string providerUserId, IDictionary<string, object> data, out string userName)
        {
            userName = providerUserId;
            return true;
        }
    }
}

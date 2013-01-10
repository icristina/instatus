using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Instatus.Integration.Server
{
    // allow users to be specified in Web.Config
    //<authentication mode="Forms">
    //  <forms loginUrl="~/Account/Login">
    //    <credentials passwordFormat="Clear">
    //      <user name="emailAddress" password="password" />
    //    </credentials>
    //  </forms>
    //</authentication>
    public class FormsAuthenticationMembership : IMembership
    {
        public bool ValidateUser(string userName, string password)
        {
            return FormsAuthentication.Authenticate(userName, password);
        }

        public bool ValidateExternalUser(string providerName, string providerUserId, IDictionary<string, object> data, out string userName)
        {
            userName = string.Empty;
            return false;
        }

        public string[] GetRoles(string userName)
        {
            return new string[] { };
        }

        public string GenerateVerificationToken(string userName)
        {
            return null;
        }

        public bool ValidateVerificationToken(string userName, string verificationToken)
        {
            return false;
        }

        public bool ChangePassword(string userName, string verificationToken, string newPassword)
        {
            return false;
        }
    }
}

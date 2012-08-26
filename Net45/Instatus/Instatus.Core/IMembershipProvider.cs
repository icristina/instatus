using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IMembershipProvider
    {
        bool ValidateUser(string userName, string password);
        bool ValidateExternalUser(string providerName, string providerUserId, IDictionary<string, string> data, out string userName);
        string[] GetRoles(string userName);
        string GenerateVerificationToken(string userName);
        bool ValidateVerificationToken(string userName, string verificationToken);
        void ChangePassword(string userName, string verificationToken, string newPassword);
    }
}

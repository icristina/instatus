﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IMembership
    {
        bool ValidateUser(string userName, string password);
        bool ValidateExternalUser(string providerName, string providerUserId, IDictionary<string, object> data, out string userName);
        string[] GetRoles(string userName);
        string GenerateVerificationToken(string userName);
        bool ValidateVerificationToken(string userName, string verificationToken);
        bool ChangePassword(string userName, string verificationToken, string newPassword);
    }
}

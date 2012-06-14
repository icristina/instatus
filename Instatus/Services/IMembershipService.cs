using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Services
{
    public interface IMembershipService
    {
        bool ValidatePassword(string username, string password);
        string[] GetRolesForUser(string username);
        string GenerateToken(string username);
        bool ValidateToken(string username, string token);
        bool VerifyUser(string username, string token);
        void ChangePassword(string username, string password);        
    }
}

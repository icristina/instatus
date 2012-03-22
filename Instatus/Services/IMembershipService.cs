using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Services
{
    public interface IMembershipService
    {
        bool ValidateUser(string username, string password);
        string[] GetRolesForUser(string username);
    }
}

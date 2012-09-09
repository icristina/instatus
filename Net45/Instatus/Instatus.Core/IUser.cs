using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IUser
    {
        string FirstName { get; }
        string LastName { get; }
        string EmailAddress { get; }
        string Locale { get; }
    }
}

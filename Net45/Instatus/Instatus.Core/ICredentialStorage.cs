using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface ICredentialStorage
    {
        ICredential GetCredential(string providerName);
        ICredential GetCredential(string providerName, string userName);
        void SaveCredential(string providerName, string userName, ICredential credential);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface ICredential
    {
        string AccountName { get; }
        string PublicKey { get; }
        string PrivateKey { get; }
        DateTime ExpiryTime { get; }
        string[] Claims { get; }
    }
}

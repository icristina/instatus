using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryCredential : ICredential
    {
        public string AccountName { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public DateTime ExpiryTime { get; set; }
        public string[] Claims { get; set; }
    }
}

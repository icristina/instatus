using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Helpers;

namespace Instatus.Integration.Server
{
    public class WebEncryption : IEncryption
    {
        public string Encrypt(string input)
        {
            return Crypto.Hash(input);
        }

        public string Decrypt(string input)
        {
            return input;
        }
    }
}

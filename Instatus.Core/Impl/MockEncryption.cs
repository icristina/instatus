using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.Impl
{
    public class MockEncryption : IEncryption
    {
        public string Encrypt(string input)
        {
            return input;
        }

        public string Decrypt(string input)
        {
            return input;
        }
    }
}

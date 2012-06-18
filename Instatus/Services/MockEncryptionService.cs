using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Services
{
    public class MockEncryptionService : IEncryptionService
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
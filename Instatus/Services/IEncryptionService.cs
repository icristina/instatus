using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string input);
        string Decrypt(string input);
    }
}

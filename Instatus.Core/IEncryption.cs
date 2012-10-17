using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface IEncryption
    {
        string Encrypt(string input);
        string Decrypt(string input);
    }
}

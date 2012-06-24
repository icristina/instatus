using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface ITokenStorage
    {
        IToken GetToken(string providerName, string userName);
        void SaveToken(string providerName, string userName, IToken token);
    }
}

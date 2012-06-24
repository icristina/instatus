using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface IToken
    {
        string AccessToken { get; }
        DateTime ExpiryTime { get; }
        string[] Claims { get; }
    }
}

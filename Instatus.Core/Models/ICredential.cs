using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Models;

namespace Instatus.Models
{
    public interface ICredential
    {
        string Key { get; }
        string Secret { get; }
        string Alias { get; }
        string Scope { get; }
        string Features { get; }
    }
}

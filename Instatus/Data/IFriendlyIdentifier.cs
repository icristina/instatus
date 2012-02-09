using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Data
{
    public interface IFriendlyIdentifier
    {
        string Slug { get; }
    }
}

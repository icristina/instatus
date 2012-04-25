using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public interface INavigableContent
    {
        string Name { get; }
        string Picture { get; }
        int Priority { get; }
    }
}
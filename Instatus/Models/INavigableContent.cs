using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;

namespace Instatus.Models
{
    public interface INavigableContent : INamed
    {
        string Name { get; }
        string Picture { get; }
        int Priority { get; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public interface INavigableContent
    {
        string Name { get; }
        string Picture { get; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public interface ITimestamp
    {
        DateTime CreatedTime { get; }
    }
}
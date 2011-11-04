using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public interface IExtensionPoint
    {
        dynamic Extensions { get; set; }
    }
}

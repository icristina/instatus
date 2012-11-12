using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Scaffold.Entities
{
    public interface IModerated
    {
        State State { get; set; }
    }
}

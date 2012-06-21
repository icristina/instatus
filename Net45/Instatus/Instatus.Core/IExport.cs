using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IExport
    {
        object DefaultConfiguration { get; }
        IEnumerable Query(object configuration);
    }
}

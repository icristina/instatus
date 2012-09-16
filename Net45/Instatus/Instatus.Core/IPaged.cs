using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IPaged : IEnumerable
    {
        int TotalItemCount { get; }
        int PageIndex { get; }
        int PageSize { get; }
    }
}

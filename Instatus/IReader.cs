using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus
{
    public interface IReader
    {
        bool CanRead(string uri);
        Task<IList> GetListAsync(string uri);
    }
}

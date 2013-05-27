using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus
{
    public interface IAuthor
    {
        bool IsSupported(string uri);
        object CreateInstance();
        Task<string> PostAsync(object model);
    }
}

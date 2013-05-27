using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus
{
    public interface IEditor
    {
        bool IsSupported(string uri);
        Task<object> GetEditorAsync(string uri);
        Task PutAsync(string uri, object model);
        Task DeleteAsync(string uri);
    }
}

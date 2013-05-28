using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus
{
    public interface IEditor
    {
        bool CanEdit(string uri);
        bool CanDelete(string uri);
        Task<object> GetEditorAsync(string uri);
        Task PutAsync(string uri, object model);
        Task DeleteAsync(string uri);
    }
}

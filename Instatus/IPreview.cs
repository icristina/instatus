using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus
{
    public interface IPreview
    {
        bool CanPreview(string uri);
        Task<object> GetPreviewAsync(string uri, object model);
    }
}

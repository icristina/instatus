using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus
{
    public interface IModerator
    {
        bool CanModerate(string uri);
        Task MarkApprovedAsync(string uri);
        Task MarkArchivedAsync(string uri);
        Task MarkSpamAsync(string uri);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus
{
    public interface IFavorites
    {
        Task<Dictionary<string, string>> GetFavoritesAsync();
    }
}

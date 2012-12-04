using Instatus.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Models
{
    public class Grid : PagedViewModel<Tile>
    {
        public Grid(IOrderedQueryable<Tile> tiles, int pageIndex, int pageSize)
            : base(tiles, pageIndex, pageSize)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.ViewModels
{
    public class TileViewModel : BindableBase
    {
        public string Uri { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

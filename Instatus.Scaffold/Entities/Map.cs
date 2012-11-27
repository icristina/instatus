using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Map
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DbGeography CenterPoint { get; set; }
        public int ZoomLevel { get; set; }

        // Associations
        public int? CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        public virtual ICollection<Marker> Markers { get; set; }

        public Map()
        {
            // initialize collections
            Markers = new List<Marker>();
        }
    }
}
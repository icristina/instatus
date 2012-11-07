using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Map : IPage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public DateTime Active { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Locale { get; set; }
        public string Category { get; set; }
        public DateTime Created { get; set; }
        public string Data { get; set; }
        public DbGeography CenterPoint { get; set; }
        public int ZoomLevel { get; set; }
        public virtual ICollection<Marker> Markers { get; set; }

        public Map()
        {
            Active = SqlDateTime.MinValue.Value;
            Start = SqlDateTime.MinValue.Value;
            End = SqlDateTime.MaxValue.Value;
            Created = DateTime.UtcNow;
        }
    }
}
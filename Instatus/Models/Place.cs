using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Instatus.Web;

namespace Instatus.Models
{
    public class Place : Page
    {
        public Address Address { get; set; }
        public Point Point { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public override WebEntry ToWebEntry() {
            return new WebGeospatialEntry()
            {
                Title = Name,
                Description = Description,
                Caption = Address.Locality,
                Picture = Picture,
                Latitude = Point.Latitude,
                Longitude = Point.Longitude
            };
        }

        public Place()
        {
            Address = new Address();
            Point = new Point();
        }
    }
}
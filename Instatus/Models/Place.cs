using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Models
{
    public class Place : Page
    {
        public Address Address { get; set; }
        public Point Point { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public Place() : base()
        {
            Address = new Address();
            Point = new Point();
        }
    }
}
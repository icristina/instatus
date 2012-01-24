using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Models
{
    [ComplexType]
    public class Point
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Zoom { get; set; }

        public bool HasCoordinates
        {
            get
            {
                return !(Latitude == 0 && Longitude == 0);
            }
        }
    }
}
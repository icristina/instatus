using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;

namespace Instatus.Models
{
    public class Journey : Checkin
    {
        public Point From { get; set; }
        public int? Distance { get; set; }

        public Journey()
        {
            Verb = WebVerb.Journey.ToString();
        }
    }
}
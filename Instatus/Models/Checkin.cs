using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Web;

namespace Instatus.Models
{
    public class Checkin : Activity
    {
        public Checkin()
            : base()
        {
            Verb = WebVerb.Checkin.ToString();
        }
    }
}

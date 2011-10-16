using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Web;

namespace Instatus.Models
{
    public class Award : Activity
    {
        public Award()
            : base()
        {
            Verb = WebVerb.Award.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Phrase
    {
        public int Id { get; set; }
        public int Locale { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Application Application { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0} = ""{1}"" ({2})", Name, Value, new CultureInfo(Locale).Name);
        }
    }
}

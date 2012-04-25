using System;
using System.Collections.Generic;
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
    }
}

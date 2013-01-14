using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Phrase
    {
        public int Id { get; set; }
        public string Locale { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
    }
}
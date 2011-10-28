using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Phrase
    {
        public int Id { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual Application Application { get; set; }
        public int? ApplicationId { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Value);
        }

        public Phrase() { }

        public Phrase(object name, string value, string locale = null)
        {
            Name = name.ToString();
            Value = value;
            Locale = locale;
        }
    }
}
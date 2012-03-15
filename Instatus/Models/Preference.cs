using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;

namespace Instatus.Models
{
    public class Preference : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Value);
        }

        public Preference(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public Preference()
        {

        }
    }
}
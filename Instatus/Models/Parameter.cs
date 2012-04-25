using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public Parameter() { }

        public Parameter(object name, object content)
        {
            Name = name.AsString();
            Content = content.AsString();
        }
    }
}

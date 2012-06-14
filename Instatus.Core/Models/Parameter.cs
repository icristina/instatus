using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Data;

namespace Instatus.Models
{
    public class Parameter : INamed
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public Parameter() { }

        public Parameter(object name, object content)
        {
            if (name != null)
                Name = name.ToString();

            if (content != null)
                Content = content.ToString();
        }
    }
}

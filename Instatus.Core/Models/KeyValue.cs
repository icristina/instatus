using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Models
{
    public class KeyValue<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
    }
}

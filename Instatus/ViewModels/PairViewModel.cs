using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.ViewModels
{
    public class PairViewModel<T>
    {
        public T Value { get; set; }
        public string Text { get; set; }
    }
}

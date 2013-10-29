using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Server
{
    public class SelectListItemModel
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public SelectListItemModel()
        {

        }

        public SelectListItemModel(string text, object value)
        {
            Text = text;
            Value = value;
        }
    }
}

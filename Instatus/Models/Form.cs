using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Form
    {
        public string ActionText { get; set; }
        public string ActionName { get; set; }
        public IList<Parameter> HiddenParameters { get; set; }

        public static Form Edit()
        {
            return new Form()
            {
                ActionText = "Edit",
                ActionName = "Edit"
            };
        }

        public static Form Create()
        {
            return new Form()
            {
                ActionText = "Save",
                ActionName = "Create"
            };
        }
    }
}

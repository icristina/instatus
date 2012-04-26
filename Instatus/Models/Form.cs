using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Web;

namespace Instatus.Models
{
    public class Form
    {
        public string ActionText { get; set; }
        public string ActionName { get; set; }

        private List<Parameter> hiddenParameters;

        public List<Parameter> HiddenParameters
        {
            get
            {
                if (hiddenParameters == null)
                    hiddenParameters = new List<Parameter>();

                return hiddenParameters;
            }
            set
            {
                hiddenParameters = value;
            }
        }

        public static Form Edit()
        {
            return new Form()
            {
                ActionText = "Edit",
                ActionName = WebPhrase.Submit
            };
        }

        public static Form Create()
        {
            return new Form()
            {
                ActionText = "Save",
                ActionName = WebPhrase.Submit
            };
        }
    }
}

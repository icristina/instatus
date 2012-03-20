using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebForm
    {
        public string ActionText { get; set; }
        public string ActionName { get; set; }

        private List<WebParameter> hiddenParameters;

        public List<WebParameter> HiddenParameters
        {
            get
            {
                if (hiddenParameters == null)
                    hiddenParameters = new List<WebParameter>();

                return hiddenParameters;
            }
            set
            {
                hiddenParameters = value;
            }
        }

        public static WebForm Create()
        {
            return new WebForm()
            {
                ActionName = WebAction.Create.ToString(),
                ActionText = WebPhrase.Create
            };
        }

        public static WebForm Edit()
        {
            return new WebForm()
            {
                ActionName = WebAction.Edit.ToString(),
                ActionText = WebPhrase.Submit
            };
        }
    }
}
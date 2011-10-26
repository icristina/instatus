using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebParameter
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public WebParameter() { }

        public WebParameter(object name, object content)
        {
            Name = name.ToString();
            Content = content.ToString();
        }

        public WebParameter(string ns, object name, object content)
        {
            Name = ns.IsEmpty() ? name.ToString() : string.Format("{0}:{1}", ns, name);
            Content = content.ToString();
        }
    }
}
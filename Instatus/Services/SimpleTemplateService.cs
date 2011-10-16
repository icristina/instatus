using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;

namespace Instatus.Services
{
    [Export(typeof(ITemplateService))]    
    public class SimpleTemplateService : ITemplateService
    {
        public string Process(string template, dynamic viewData)
        {
            return viewData.ToString().Replace("\n", "<br/>");
        }
    }
}
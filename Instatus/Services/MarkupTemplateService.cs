﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using Instatus;
using Instatus.Web;

namespace Instatus.Services
{
    [Export(typeof(ITemplateService))]    
    public class MarkupTemplateService : ITemplateService
    {
        public string Process(string template, dynamic viewData)
        {
            string markup = viewData.ToString();

            markup = markup.RewriteRelativePaths();

            // require block level container
            if (!markup.Contains("<p") && !markup.Contains("<div"))
            {
                markup = string.Format("<p>{0}</p>", markup);
            }                
            
            return markup;
        }
    }
}
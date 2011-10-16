using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Services
{
    public interface ITemplateService
    {
        string Process(string template, dynamic viewData);
    }
}
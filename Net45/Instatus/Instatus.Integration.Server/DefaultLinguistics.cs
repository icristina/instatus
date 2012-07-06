using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using Instatus.Core;

namespace Instatus.Integration.Server
{
    public class DefaultLinguistics : ILinguistics
    {
        private PluralizationService pluralizationService = PluralizationService.CreateService(new CultureInfo("en-GB"));

        public string Pluralize(string text)
        {
            return pluralizationService.Pluralize(text);
        }

        public string Singularize(string text)
        {
            return pluralizationService.Singularize(text);
        }

        public string Suggestions(string text)
        {
            throw new NotImplementedException();
        }
    }
}

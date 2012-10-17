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
        private IPreferences preferences;

        public string Plural(string text)
        {
            return PluralizationService.CreateService(new CultureInfo(preferences.Locale)).Pluralize(text);
        }

        public string Singular(string text)
        {
            return PluralizationService.CreateService(new CultureInfo(preferences.Locale)).Singularize(text);
        }

        public DefaultLinguistics(IPreferences preferences)
        {
            this.preferences = preferences;
        }
    }
}

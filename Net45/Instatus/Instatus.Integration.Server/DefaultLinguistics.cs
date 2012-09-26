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
        private ISessionData sessionData;

        public string Plural(string text)
        {
            return PluralizationService.CreateService(new CultureInfo(sessionData.Locale)).Pluralize(text);
        }

        public string Singular(string text)
        {
            return PluralizationService.CreateService(new CultureInfo(sessionData.Locale)).Singularize(text);
        }

        public DefaultLinguistics(ISessionData sessionData)
        {
            this.sessionData = sessionData;
        }
    }
}

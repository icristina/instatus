using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public abstract class ExtendedViewPage : WebViewPage
    {
        private ILocalization localization;
        private ISessionData sessionData;
        
        // automatic autofac di does not appear to work with razorgenerator
        public ILocalization Localization 
        {
            get
            {
                return localization ?? (localization = DependencyResolver.Current.GetService<ILocalization>());
            }
        }

        public ISessionData SessionData 
        {
            get
            {
                return sessionData ?? (sessionData = DependencyResolver.Current.GetService<ISessionData>());
            }
        }

        public string Phrase(string key)
        {
            return Localization.Phrase(key);
        }

        public string Format(string key, params object[] values)
        {
            return Localization.Format(key, values);
        }
    }

    public abstract class ExtendedViewPage<T> : WebViewPage<T>
    {
        private ILocalization localization;
        private ISessionData sessionData;

        public ILocalization Localization
        {
            get
            {
                return localization ?? (localization = DependencyResolver.Current.GetService<ILocalization>());
            }
        }

        public ISessionData SessionData
        {
            get
            {
                return sessionData ?? (sessionData = DependencyResolver.Current.GetService<ISessionData>());
            }
        }

        public string Phrase(string key)
        {
            return Localization.Phrase(key);
        }

        public string Format(string key, params object[] values)
        {
            return Localization.Format(key, values);
        }
    }
}

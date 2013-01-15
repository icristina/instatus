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
        private IGlobalization globalization;
        private ILocalization localization;
        private IPreferences preferences;
        private IHosting hosting;
        
        // automatic autofac di does not appear to work with razorgenerator
        public IGlobalization Globalization
        {
            get
            {
                return globalization ?? (globalization = DependencyResolver.Current.GetService<IGlobalization>());
            }
        }

        public ILocalization Localization 
        {
            get
            {
                return localization ?? (localization = DependencyResolver.Current.GetService<ILocalization>());
            }
        }

        public IPreferences Preferences 
        {
            get
            {
                return preferences ?? (preferences = DependencyResolver.Current.GetService<IPreferences>());
            }
        }

        public IHosting Hosting
        {
            get
            {
                return hosting ?? (hosting = DependencyResolver.Current.GetService<IHosting>());
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
        private IGlobalization globalization;
        private ILocalization localization;
        private IPreferences preferences;
        private IHosting hosting;

        public IGlobalization Globalization
        {
            get
            {
                return globalization ?? (globalization = DependencyResolver.Current.GetService<IGlobalization>());
            }
        }

        public ILocalization Localization
        {
            get
            {
                return localization ?? (localization = DependencyResolver.Current.GetService<ILocalization>());
            }
        }

        public IPreferences Preferences
        {
            get
            {
                return preferences ?? (preferences = DependencyResolver.Current.GetService<IPreferences>());
            }
        }

        public IHosting Hosting
        {
            get
            {
                return hosting ?? (hosting = DependencyResolver.Current.GetService<IHosting>());
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

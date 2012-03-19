using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Instatus.Data
{
    public interface IRule<TContext, TResult> : INamed
    {
        TResult Evaluate(TContext context);
    }
    
    public interface IRule<TContext> : IRule<TContext, bool>
    {

    }

    public class RegexRule : IRule<string>
    {
        private bool positiveMatch;
        private Regex regex;

        public string Name
        {
            get
            {
                return regex.GetHashCode().AsString();
            }
        }

        public bool Evaluate(string instance)
        {
            var isMatch = regex.IsMatch(instance);
            return positiveMatch ? isMatch : !isMatch;
        }

        public RegexRule(string pattern, bool positiveMatch = true)
        {
            this.positiveMatch = positiveMatch;
            this.regex = new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }

    public class DeligateRule<T> : IRule<T>
    {
        private Func<T, bool> deligate;

        public string Name
        {
            get
            {
                return deligate.GetHashCode().AsString();
            }
        }

        public bool Evaluate(T instance)
        {
            return deligate(instance);
        }

        public DeligateRule(Func<T, bool> deligate)
        {
            this.deligate = deligate;
        }
    }
}
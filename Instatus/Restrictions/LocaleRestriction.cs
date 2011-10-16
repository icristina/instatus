using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using System.ComponentModel.Composition;

namespace Instatus.Restrictions
{
    [Export(typeof(IRestrictionEvaluator))]
    [PartCreationPolicy(CreationPolicy.NonShared)]    
    public class LocaleRestriction : BaseRestrictionEvaluator
    {
        public override RestrictionResult Evaluate(RestrictionContext context, string data)
        {
            return RestrictionResult.Valid(context.User.Locale == data);
        }

        public LocaleRestriction(string countryCode) : base(1, countryCode) { }

        public LocaleRestriction() { }
    }
}
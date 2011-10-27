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
    public class LocaleRestriction : BaseRestrictionEvaluator<string>
    {
        public override RestrictionResult Evaluate(RestrictionContext context)
        {
            return RestrictionResult.Valid(context.User.Locale == Value);
        }

        public LocaleRestriction(string countryCode) {
            Value = countryCode;
        }

        public LocaleRestriction() { }
    }
}
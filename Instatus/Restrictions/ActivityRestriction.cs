using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using System.ComponentModel.Composition;
using Instatus.Data;
using Instatus.Web;

namespace Instatus.Restrictions
{
    [Export(typeof(IRestrictionEvaluator))]
    [PartCreationPolicy(CreationPolicy.NonShared)]    
    public class ActivityRestriction : BaseRestrictionEvaluator<Tuple<WebVerb, bool>>
    {
        public override RestrictionResult Evaluate(RestrictionContext context)
        {
            return RestrictionResult.Valid(!(context.Trigger.Verb == Value.Item1.ToString() && !Value.Item2));
        }

        public ActivityRestriction(WebVerb verb, bool enabled = true) {
            Value = new Tuple<WebVerb, bool>(verb, enabled);
        }

        public ActivityRestriction() { }
    }
}
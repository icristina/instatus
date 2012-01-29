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
    public class ActivityRestriction : BaseRestrictionEvaluator<WebVerb>
    {
        public override RestrictionResult Evaluate(RestrictionContext context)
        {
            return RestrictionResult.Valid(context.Trigger.Verb == Value.ToString());
        }

        public ActivityRestriction(WebVerb verb) {
            Value = verb;
        }

        public ActivityRestriction() { }
    }
}
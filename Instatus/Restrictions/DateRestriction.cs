using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;
using System.ComponentModel.Composition;
using Instatus.Data;

namespace Instatus.Restrictions
{
    [Export(typeof(IRestrictionEvaluator))]
    [PartCreationPolicy(CreationPolicy.NonShared)]    
    public class DateRestriction : BaseRestrictionEvaluator<Range<DateTime>>
    {
        public override RestrictionResult Evaluate(RestrictionContext context)
        {
            return RestrictionResult.Valid(context.Trigger.CreatedTime >= Value.Start && context.Trigger.CreatedTime <= Value.End);
        }

        public DateRestriction(DateTime start, DateTime end) {
            Value = new Range<DateTime>(start, end);
        }

        public DateRestriction() { }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using Instatus.Models;

namespace Instatus.Restrictions
{
    [Export(typeof(IRestrictionEvaluator))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RoleRestriction : BaseRestrictionEvaluator 
    {
        public override RestrictionResult Evaluate(RestrictionContext context, string data)
        {
            return RestrictionResult.Valid(HttpContext.Current.User.IsInRole(data));
        }
        
        public RoleRestriction(object roleName) : base(1, roleName.ToString()) { }

        public RoleRestriction() { }
    }
}
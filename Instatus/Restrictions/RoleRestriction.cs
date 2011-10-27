using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using Instatus.Models;
using Instatus.Web;

namespace Instatus.Restrictions
{
    [Export(typeof(IRestrictionEvaluator))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RoleRestriction : BaseRestrictionEvaluator<string>
    {
        public override RestrictionResult Evaluate(RestrictionContext context)
        {
            return RestrictionResult.Valid(HttpContext.Current.User.IsInRole(Value));
        }
        
        public RoleRestriction(string roleName) {
            Value = roleName;
        }

        public RoleRestriction(WebRole role)
        {
            Value = role.ToString();
        }

        public RoleRestriction() { }
    }
}
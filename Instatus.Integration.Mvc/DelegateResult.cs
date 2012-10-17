using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public class DelegateResult : ActionResult
    {
        private Action<ControllerContext> action;
        
        public override void ExecuteResult(ControllerContext context)
        {
            action(context);
        }

        public DelegateResult(Action<ControllerContext> action) 
        {
            this.action = action;
        }
    }
}

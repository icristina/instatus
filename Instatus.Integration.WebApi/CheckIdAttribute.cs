using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core.Extensions;

namespace Instatus.Integration.WebApi
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CheckIdAttribute : CheckModelForNullAttribute
    {
        public CheckIdAttribute()
            : base(attributes => { return attributes.ContainsKey("id") && attributes.GetValue<int>("id") < 1; })
        {

        }
    }
}

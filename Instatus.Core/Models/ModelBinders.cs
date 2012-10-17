using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core.Extensions;

namespace Instatus.Core.Models
{
    public static class ModelBinders
    {
        public static IDictionary<Type, Func<IDictionary<string, object>, object>> Binders = new Dictionary<Type, Func<IDictionary<string, object>, object>>()
        {
            { typeof(Credential), values => {
                return new Credential()
                {
                    AccountName = values.GetValue<string>("AccountName"),
                    PrivateKey = values.GetValue<string>("PrivateKey"),
                    PublicKey = values.GetValue<string>("PublicKey"),
                    Claims = (values.GetValue<string>("Claims") ?? "").Split(',')
                };
            }}
        };
    }
}

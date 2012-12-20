using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Scaffold.Entities
{
    public static class PayloadExtensions
    {
        public static void SetPayload<T>(this IPayload entity, T obj, IJsonSerializer jsonSerializer = null)
        {
            entity.Data = (jsonSerializer ?? DependencyResolver.Current.GetService<IJsonSerializer>()).Stringify(obj);
        }

        public static T GetPayload<T>(this IPayload entity, IJsonSerializer jsonSerializer = null)
        {
            return (jsonSerializer ?? DependencyResolver.Current.GetService<IJsonSerializer>()).Parse<T>(entity.Data);
        }
    }
}
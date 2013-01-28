using Instatus.Core;
using Instatus.Scaffold.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Scaffold
{
    public static class PayloadExtensions
    {
        private static IJsonSerializer GetSerializer(IJsonSerializer jsonSerializer)
        {
            return jsonSerializer ?? DependencyResolver.Current.GetService<IJsonSerializer>();
        }
        
        public static void SetPayload<T>(this IPayload entity, T obj, IJsonSerializer jsonSerializer = null)
        {
            entity.Data = GetSerializer(jsonSerializer).Stringify(obj);
        }

        public static T GetPayload<T>(this IPayload entity, IJsonSerializer jsonSerializer = null)
        {
            var payload = GetSerializer(jsonSerializer).Parse<T>(entity.Data);

            return payload == null ? Activator.CreateInstance<T>() : payload;
        }
    }
}
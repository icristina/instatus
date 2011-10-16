using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace Instatus
{
    public static class ControllerDescriptorExtensions
    {
        public static string Description(this ControllerDescriptor descriptor)
        {
            return descriptor.GetAttribute<DescriptionAttribute>().Description;
        }

        public static T GetAttribute<T>(this ControllerDescriptor descriptor) where T : class
        {
            var attributes = descriptor.GetCustomAttributes(typeof(T), true);
            return attributes.FirstOrDefault() as T;
        }

        public static bool HasAction(this ControllerDescriptor descriptor, string actionName)
        {
            return descriptor.GetCanonicalActions().Any(a => a.ActionName == actionName);
        }
    }
}
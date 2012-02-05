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
        public static ControllerDescriptor Descriptor(this IController controller)
        {
            return new ReflectedControllerDescriptor(controller.GetType());
        }
        
        public static string Description(this ControllerDescriptor descriptor)
        {
            var description = descriptor.GetAttribute<DescriptionAttribute>();
            return description != null ? description.Description : string.Empty;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Instatus.Models;
using Instatus.Data;

namespace Instatus.Data
{
    public interface IExtensionPoint
    {
        dynamic Extensions { get; set; }
    }
}

namespace Instatus
{
    public static class ExtensionPointExtensions
    {
        public static IEnumerable<T> ExtensionsAll<T>(this IExtensionPoint extensionPoint)
        {
            return ((IDictionary<string, object>)extensionPoint.Extensions)
                    .Where(k => k.Value is IEnumerable<T>)
                    .SelectMany(k => k.Value as IEnumerable<T>);
        }
    }
}

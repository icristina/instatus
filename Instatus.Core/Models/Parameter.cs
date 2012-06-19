using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Data;
using Instatus.Models;

namespace Instatus.Models
{
    public class Parameter : INamed
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public Parameter() { }

        public Parameter(object name, object content)
        {
            if (name != null)
                Name = name.ToString();

            if (content != null)
                Content = content.ToString();
        }
    }
}

namespace Instatus
{
    public static class ParameterExtensions
    {
        public static TValue Value<T, TValue>(this IEnumerable<T> source, string name, Func<T, TValue> accessor) where T : INamed
        {
            return source.Where(s => s.Name.Match(name)).Select(accessor).FirstOrDefault();
        }

        public static Parameter Get(this ICollection<Parameter> source, string name)
        {
            Parameter item = source.Where(s => s.Name.Match(name)).FirstOrDefault();

            if (item == null)
            {
                item = new Parameter();
                item.Name = name;
                source.Add(item);
            }

            return item;
        }
    }
}

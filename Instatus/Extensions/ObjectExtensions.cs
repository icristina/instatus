using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using System.Web.Mvc;
using System.Web.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Hosting;
using System.Collections.Specialized;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Instatus.Data;
using Instatus.Web;
using System.Reflection;
using System.Data.Entity;

namespace Instatus
{
    public static class ObjectExtensions
    {
        public static TValue GetCustomAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> predicate) where TAttribute : class
        {
            var attribute = TypeDescriptor.GetAttributes(type).OfType<TAttribute>().LastOrDefault() as TAttribute; // last ensures TypeDescriptor.AddAttributes(typeof(TypeName), new Attribute("Value")) allowed in global.asa
            return attribute != null ? predicate(attribute) : default(TValue);
        }        
        
        public static TValue GetCustomAttributeValue<TAttribute, TValue>(this object graph, Func<TAttribute, TValue> predicate) where TAttribute : class
        {
            return graph.GetType().GetCustomAttributeValue(predicate);
        }
        
        public static NameValueCollection ToNameValueCollection(this object graph, bool lowerCasePropertyNames = true)
        {
            var nameValueCollection = new NameValueCollection();

            foreach(var keyValuePair in new RouteValueDictionary(graph).Where(v => v.Value.NonEmpty())) {
                var key = keyValuePair.Key;

                if (lowerCasePropertyNames)
                    key = key.ToLower();
                
                nameValueCollection.Add(key, keyValuePair.Value.ToString());
            }
            
            return nameValueCollection;
        }
        
        public static bool IsValid<T>(this T graph, bool throwError = false)
        {
            var validationContext = new ValidationContext(graph, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(graph, validationContext, results, true);

            if(throwError && !isValid)
                throw new ValidationException();

            return isValid;
        }
        
        public static byte[] Serialize<T>(this T graph, IEnumerable<Type> knownTypes = null)
        {
            var stream = new MemoryStream();
            var serializer = new DataContractSerializer(typeof(T), knownTypes);
            serializer.WriteObject(stream, graph);
            return stream.ToArray();
        }

        public static T Deserialize<T>(this byte[] bytes, IEnumerable<Type> knownTypes = null)
        {
            if (bytes == null)
                return default(T);
            
            MemoryStream stream = new MemoryStream(bytes);
            stream.Position = 0;
            var serializer = new DataContractSerializer(typeof(T), knownTypes);
            return (T)serializer.ReadObject(stream);
        }

        public static void Save(this object instance, string virtualPath)
        {
            using (var fs = new FileStream(HostingEnvironment.MapPath(virtualPath), FileMode.OpenOrCreate, FileAccess.Write))
            {
                DataContractSerializer ser = new DataContractSerializer(instance.GetType());
                ser.WriteObject(fs, instance);
            }
        }

        public static T AssertNotEmpty<T>(this T graph, string message = "Null reference")
        {
            if (graph.IsEmpty())
                throw new Exception(message);

            return graph;
        }

        public static bool IsEmpty(this object graph)
        {
            return graph == null 
                || (graph is IHasValue && !((IHasValue)graph).HasValue)
                || (graph is string && string.IsNullOrWhiteSpace((string)graph)) 
                || (graph is ICollection && ((ICollection)graph).Count == 0)
                || (graph is IEnumerable && EnumerableExtensions.Count(((IEnumerable)graph)) == 0)
                || (graph is DateTime && (DateTime)graph == DateTime.MinValue)
                || (graph is HttpPostedFileBase && ((HttpPostedFileBase)graph).ContentLength == 0);
        }

        public static bool NonEmpty(this object graph)
        {
            return !graph.IsEmpty();
        }

        public static MvcHtmlString ToAttr(this object graph, string attributeName)
        {
            return graph.ToAttr(attributeName, "{0}");
        }

        public static MvcHtmlString ToAttr(this object graph, string attributeName, string formatString)
        {
            return graph.IsEmpty() ? null : new MvcHtmlString(string.Format("{0}=\"{1}\"", attributeName, string.Format(formatString, graph)));
        }

        public static string AsString(this object graph)
        {
            return graph.IsEmpty() ? string.Empty : graph.ToString();
        }

        public static bool AsBoolean(this object graph, bool defaultValue = false)
        {
            bool result;
            return bool.TryParse(graph.AsString(), out result) ? result : defaultValue;
        }

        public static int AsInteger(this object graph)
        {
            int result;

            if (int.TryParse(graph.AsString(), out result))
                return result;
            
            return 0;
        }

        public static double AsDouble(this object graph)
        {
            double result;

            if (double.TryParse(graph.AsString(), out result))
                return result;

            return 0;
        }

        public static string ToJson(this object graph)
        {
            if (graph is IDictionary<string, object>)
                return new JavaScriptSerializer().Serialize(graph);
            
            return Json.Encode(graph);
        }

        public static T OrDefault<T>(this T value, T defaultValue) {
            return value.IsEmpty() ? defaultValue : value;
        }

        public static object GetKey(this object graph)
        {
            var properties = TypeDescriptor.GetProperties(graph);

            foreach (PropertyDescriptor property in properties)
            {
                if (property.Attributes.OfType<KeyAttribute>().Count() > 0)
                {
                    return property.GetValue(graph);
                }
            }
            
            var keyNameConventions = new string[] { "Id", graph.GetType().Name + "Id" };

            foreach(var propertyName in keyNameConventions) {
                var property = properties.Find(propertyName, true);
                if(property != null)
                    return property.GetValue(graph);
            }

            return null;
        }

        public static string GetVocabularyItemType(this object graph)
        {
            var vocabulary = WebApp.GetService<IWebVocabulary>();
            return vocabulary == null ? string.Empty : vocabulary.GetItemType(graph);
        }

        public static T FirstMemberOfType<T>(this object graph) where T : class
        {
            return graph.GetType()
                    .GetProperties()
                    .Where(property => typeof(T).IsAssignableFrom(property.PropertyType))
                    .Select(property => property.GetValue(graph, null))
                    .First() as T;
        }

        public static void ActivateCollections(this object target)
        {
            foreach (var property in target.GetType().GetProperties()
                .Where(p => p.PropertyType.IsGenericType 
                    && p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)
                    && !p.HasAttribute<IgnoreDataMemberAttribute>()) // ensure tables such as logs are not accessed
                    .ToList())
            {
                if (property.GetValue(target, null) == null)
                {
                    var concreteType = typeof(List<>).MakeGenericType(property.PropertyType.GetGenericArguments()[0]);                    
                    property.SetValue(target, Activator.CreateInstance(concreteType), null);
                }                
            }
        }

        public static void ApplyAction<TPropertyType>(this object target, Action<TPropertyType> action) {
            foreach (var property in target.GetType().GetProperties())
            {
                var destinationValue = property.GetValue(target, null);

                if (destinationValue != null && destinationValue is TPropertyType)
                {
                    action((TPropertyType)destinationValue);
                }
            }
        }

        public static T SerializationSafe<T>(this T source)
        {
            if (source is DbContext)
                (source as DbContext).DisableProxiesAndLazyLoading();
            
            return source;
        }

        public static TTarget ApplyValues<TTarget, TSource>(this TTarget target, TSource source, bool recursive = false, string[] exclusions = null)
        {
            // each property with matching name in target and source
            foreach (var property in source.GetType().GetProperties())
            {
                var destination = target.GetType().GetProperty(property.Name);
                var changeEvent = string.Format("change.{0}", property.Name);

                if (WebApp.Can(changeEvent, target) && !(exclusions != null && exclusions.Contains(property.Name)) && destination != null && destination.CanWrite)
                {
                    var destinationValue = destination.GetValue(target, null);
                    
                    // if int, string or enum set value, if second level in object graph always set value even if complex type
                    // nullable types should evaluate to true if check whether destination is assignable from property
                    if ((destination.PropertyType.IsSimpleType() || !recursive) && destination.PropertyType.IsAssignableFrom(property.PropertyType))
                    {
                        var value = property.GetValue(source, null);

                        if (value is string)
                            value = ((string)value).TrimOrNull(); // auto-trim strings
                        
                        destination.SetValue(target, value, null);
                    }
                    else if (recursive)
                    {
                        var propertyValue = property.GetValue(source, null);

                        if (propertyValue == null)
                        {
                            destination.SetValue(target, null, null);
                        }
                        else
                        {
                            // activate new instance of complex types
                            if (destinationValue == null)
                            {
                                destinationValue = Activator.CreateInstance(destination.PropertyType);
                                destination.SetValue(target, destinationValue, null);
                            }

                            destinationValue.ApplyValues(propertyValue, false, exclusions);
                        }
                    }
                }
            }

            return target;
        }

        public static T MapTo<T>(this object graph, bool recursive = false)
        {
            return Activator.CreateInstance<T>().ApplyValues(graph, recursive);
        }

        public static bool IsSimpleType(this Type type)
        {
            return type.IsPrimitive || type.IsValueType || type.IsAssignableFrom(typeof(string)) || type.IsAssignableFrom(typeof(DateTime?));
        }

        public static bool TryDispose(this object graph)
        {
            using (graph as IDisposable)
            {
                return true;
            }
        }

        public static bool TryDatabind(this object graph)
        {
            if (graph is IDataboundModel)
            {
                ((IDataboundModel)graph).Databind();
            }

            return false;
        }
    }
}
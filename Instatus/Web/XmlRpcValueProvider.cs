using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Globalization;
using System.Web.Routing;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Collections;
using System.Text;

namespace Instatus.Web
{
    public class XmlRpcValueProvider : IValueProvider
    {
        private ControllerContext controllerContext;
        private XDocument xml;
        private IEnumerable<XElement> parameters;
        private int index = 0;

        public bool ContainsPrefix(string prefix)
        {
            var contentType = controllerContext.HttpContext.Request.ContentType;

            if (!contentType.StartsWith("text/xml", StringComparison.OrdinalIgnoreCase) && !contentType.StartsWith("application/xml", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            
            return true;
        }

        public ValueProviderResult GetValue(string key)
        {
            if (xml == null)
            {
                xml = XDocument.Parse(controllerContext.HttpContext.Request.InputStream.CopyToString());
                parameters = xml.Root.Element("params").Elements("param");
            }

            XElement element;

            if (key.Contains("."))
            {
                element = parameters.ElementAtOrDefault(index - 1);
            }
            else
            {
                element = parameters.ElementAtOrDefault(index);
                index++;
            }

            if (element != null)
            {
                var value = element.Element("value");
                var structure = value.Elements("struct").FirstOrDefault();

                if(structure != null) {
                    var subKey = key.SubstringAfter(".");
                    var subNode = structure.Elements().FirstOrDefault(m => m.Element("name").Value == subKey);

                    if (subNode != null)
                    {
                        var subValue = subNode.Elements().Last();
                        return new ValueProviderResult(subValue.Value, subValue.Value, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        return null;
                    }
                } else {
                    return new ValueProviderResult(value.Value, value.Value, CultureInfo.CurrentCulture);
                }
            }

            return null;
        }

        public XmlRpcValueProvider(ControllerContext controllerContext)
        {
            this.controllerContext = controllerContext;
        }
    }

    public class XmlRpcValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new XmlRpcValueProvider(controllerContext);
        }
    }

    public class XmlRpcRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (httpContext.Request.ContentLength == 0)
                return false;
            
            var xml = XDocument.Parse(httpContext.Request.InputStream.CopyToString());
            var methodName = xml.Root.Element("methodName").Value;
            values.Add("action", methodName);
            return true;
        }
    }

    public class XmlRpcResult : ActionResult
    {
        private object graph;
        
        public override void ExecuteResult(ControllerContext context)
        {
            using (var writer = XmlDictionaryWriter.CreateTextWriter(context.RequestContext.HttpContext.Response.OutputStream))
            {
                writer.WriteStartElement(XmlRpcProtocol.MethodResponse);   
                new XmlRpcSerializer().WriteObjectContent(writer, graph);
                writer.WriteEndElement();
            }
        }

        public XmlRpcResult(object graph)
        {
            this.graph = graph;
        }
    }

    public class XmlRpcSerializer : XmlObjectSerializer
    {
        private static Dictionary<string, MemberInfo> GetDataMembers(Type targetType)
        {
            Dictionary<string, MemberInfo> dataMembers = new Dictionary<string, MemberInfo>();
            foreach (MemberInfo member in targetType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetField | BindingFlags.SetProperty))
            {
                object[] attributes = member.GetCustomAttributes(typeof(DataMemberAttribute), true);
                if (attributes.Length == 1)
                {
                    DataMemberAttribute dataMember = (DataMemberAttribute)attributes[0];
                    if (!string.IsNullOrEmpty(dataMember.Name))
                    {
                        dataMembers.Add(dataMember.Name, member);
                    }
                    else
                    {
                        dataMembers.Add(member.Name, member);
                    }
                }
            }
            return dataMembers;
        }        
        
        public static void SerializeStruct(XmlDictionaryWriter writer, object value)
        {
            Type valueType = value.GetType();
            Dictionary<string, MemberInfo> dataMembers = GetDataMembers(valueType);
            if (valueType.IsDefined(typeof(DataContractAttribute), false))
            {
                writer.WriteStartElement(XmlRpcProtocol.Struct);
                foreach (KeyValuePair<string, MemberInfo> member in dataMembers)
                {
                    object elementValue = null;

                    if (member.Value is PropertyInfo)
                    {
                        elementValue = ((PropertyInfo)member.Value).GetValue(value,
                                        BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.NonPublic,
                                        null,
                                        null,
                                        CultureInfo.CurrentCulture);
                    }
                    else if (member.Value is FieldInfo)
                    {
                        elementValue = ((FieldInfo)member.Value).GetValue(value);
                    }

                    if (elementValue != null)
                    {
                        writer.WriteStartElement(XmlRpcProtocol.Member);
                        writer.WriteStartElement(XmlRpcProtocol.Name);
                        writer.WriteString(member.Key);
                        writer.WriteEndElement();
                        writer.WriteStartElement(XmlRpcProtocol.Value);
                        Serialize(writer, elementValue);
                        writer.WriteEndElement(); // value
                        writer.WriteEndElement(); // member
                    }
                }
                writer.WriteEndElement(); // struct
            }
        }

        public static void Serialize(XmlDictionaryWriter writer, object value)
        {
            if (value != null)
            {
                Type valueType = value.GetType();
                if (valueType.IsDefined(typeof(DataContractAttribute), false))
                {
                    SerializeStruct(writer, value);
                }
                else if (valueType == typeof(Int32))
                {
                    writer.WriteStartElement(XmlRpcProtocol.Integer);
                    writer.WriteValue(value);
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(double) || valueType == typeof(float))
                {
                    writer.WriteStartElement(XmlRpcProtocol.Double);
                    writer.WriteValue(((double)value).ToString("r"));
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(DateTime))
                {
                    writer.WriteStartElement(XmlRpcProtocol.DateTime);
                    writer.WriteValue(value);
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(Boolean))
                {
                    writer.WriteStartElement(XmlRpcProtocol.Bool);
                    writer.WriteValue(((bool)value) ? 1 : 0);
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(String) ||
                         valueType == typeof(TimeSpan))
                {
                    writer.WriteStartElement(XmlRpcProtocol.String);
                    writer.WriteValue(value);
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(Uri))
                {
                    writer.WriteStartElement(XmlRpcProtocol.String);
                    writer.WriteValue(((Uri)value).ToString());
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(byte[]))
                {
                    writer.WriteStartElement(XmlRpcProtocol.ByteArray);
                    writer.WriteBase64((byte[])value, 0, ((byte[])value).Length);
                    writer.WriteEndElement();
                }
                else if (valueType == typeof(Stream))
                {
                    int chunkSize = 1024 * 5;
                    byte[] buffer = new byte[chunkSize];
                    int offset = 0;
                    int bytesRead;

                    writer.WriteStartElement(XmlRpcProtocol.ByteArray);
                    do
                    {
                        bytesRead = ((Stream)value).Read(buffer, offset, buffer.Length);
                        writer.WriteBase64(buffer, 0, bytesRead);
                        offset += bytesRead;
                    }
                    while (bytesRead == buffer.Length);
                    writer.WriteEndElement();
                }
                else if (value is IEnumerable)
                {
                    writer.WriteStartElement(XmlRpcProtocol.Array);
                    writer.WriteStartElement(XmlRpcProtocol.Data);
                    foreach (object obj in (IEnumerable)value)
                    {
                        writer.WriteStartElement(XmlRpcProtocol.Value);
                        Serialize(writer, obj);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
            else
            {
                writer.WriteStartElement(XmlRpcProtocol.Nil);
                writer.WriteEndElement();
            }
        }
        
        public override bool IsStartObject(XmlDictionaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
        {
            throw new NotImplementedException();
        }

        public override void WriteEndObject(XmlDictionaryWriter writer)
        {

        }

        public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
        {
            SerializeStruct(writer, graph);
        }

        public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
        {

        }
    }

    internal class XmlRpcProtocol
    {
        internal const string MethodCall = "methodCall";
        internal const string MethodResponse = "methodResponse";
        internal const string MethodName = "methodName";
        internal const string Int32 = "i4";
        internal const string Integer = "int";
        internal const string DateTime = "dateTime.iso8601";
        internal const string String = "string";
        internal const string ByteArray = "base64";
        internal const string Bool = "boolean";
        internal const string Struct = "struct";
        internal const string Member = "member";
        internal const string Value = "value";
        internal const string Name = "name";
        internal const string Params = "params";
        internal const string Param = "param";
        internal const string Array = "array";
        internal const string Double = "double";
        internal const string Data = "data";
        internal const string Nil = "nil";
    }
}
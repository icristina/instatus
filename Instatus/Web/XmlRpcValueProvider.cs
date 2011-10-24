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
using System.ComponentModel;

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
                    var subKey = key.SubstringAfter(".").ToCamelCase();
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
            var response = context.RequestContext.HttpContext.Response;

            response.ContentType = WebContentType.Xml.ToMimeType();
            
            using (var writer = XmlDictionaryWriter.CreateTextWriter(response.OutputStream))
            {
                var model = graph ?? context.Controller.ViewData.Model;

                writer.WriteStartDocument();
                writer.WriteStartElement(XmlRpcProtocol.MethodResponse);
                writer.WriteStartElement(XmlRpcProtocol.Params);
                writer.WriteStartElement(XmlRpcProtocol.Param);
                writer.WriteStartElement(XmlRpcProtocol.Value); 
                XmlRpcSerializer.Serialize(writer, model);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public XmlRpcResult(object graph)
        {
            this.graph = graph;
        }

        public XmlRpcResult() { }
    }

    // http://vasters.com/clemensv/PermaLink,guid,679ca50b-c907-4831-81c4-369ef7b85839.aspx
    public class XmlRpcSerializer
    {        
        public static void SerializeStruct(XmlDictionaryWriter writer, object value)
        {
            writer.WriteStartElement(XmlRpcProtocol.Struct);

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value))
            {
                writer.WriteStartElement(XmlRpcProtocol.Member);
                writer.WriteStartElement(XmlRpcProtocol.Name);
                writer.WriteString(property.Name.ToCamelCase());
                writer.WriteEndElement();
                writer.WriteStartElement(XmlRpcProtocol.Value);
                Serialize(writer, property.GetValue(value));
                writer.WriteEndElement();
                writer.WriteEndElement();                              
            }
            
            writer.WriteEndElement();
        }

        public static void Serialize(XmlDictionaryWriter writer, object value)
        {
            if (value != null)
            {
                Type valueType = value.GetType();
                if (valueType == typeof(Int32))
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
                else 
                {
                    SerializeStruct(writer, value);
                }
            }
            else
            {
                writer.WriteStartElement(XmlRpcProtocol.Nil);
                writer.WriteEndElement();
            }
        }
    }

    public class XmlRpcProtocol
    {
        public const string MethodCall = "methodCall";
        public const string MethodResponse = "methodResponse";
        public const string MethodName = "methodName";
        public const string Int32 = "i4";
        public const string Integer = "int";
        public const string DateTime = "dateTime.iso8601";
        public const string String = "string";
        public const string ByteArray = "base64";
        public const string Bool = "boolean";
        public const string Struct = "struct";
        public const string Member = "member";
        public const string Value = "value";
        public const string Name = "name";
        public const string Params = "params";
        public const string Param = "param";
        public const string Array = "array";
        public const string Double = "double";
        public const string Data = "data";
        public const string Nil = "nil";
    }
}
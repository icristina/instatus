using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core.Extensions;

namespace Instatus.Core.Impl
{
    public class DataContractSerializerImpl : ISerializer
    {
        private Type[] knownTypes;

        public T Deserialize<T>(byte[] bytes)
        {
            if (bytes == null)
                return default(T);

            using (var stream = new MemoryStream(bytes))
            {
                stream.ResetPosition();
                var serializer = new DataContractSerializer(typeof(T), knownTypes);
                return (T)serializer.ReadObject(stream);
            }
        }

        public byte[] Serialize(object graph)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(graph.GetType(), knownTypes);
                serializer.WriteObject(stream, graph);
                return stream.ToArray();
            }
        }

        public DataContractSerializerImpl(Type[] knownTypes)
        {
            this.knownTypes = knownTypes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.Extensions
{
    public static class StorageExtensions
    {
        public static void LoadFromFile<T>(this IEntitySet<T> set, string virtualPath, IBlobStorage blobStorage, ISerializer serializer) where T : class
        {
            using (var fileStream = blobStorage.OpenRead(virtualPath))
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);

                var posts = serializer.Deserialize<List<T>>(memoryStream.ToArray());

                foreach (var post in posts)
                {
                    set.Add(post);
                }
            };
        }
    }
}

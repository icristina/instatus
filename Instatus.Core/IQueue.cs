using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IQueue<T>
    {
        void Enqueue(T message);
        bool TryDequeue(out T message);
    }
}

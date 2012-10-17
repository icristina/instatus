using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IMessageBus
    {
        void Subscribe<T>(Action<T> action);
        void Publish<T>(T message);
    }
}

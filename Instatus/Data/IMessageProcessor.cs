using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Instatus.Services;
using Instatus.Web;

namespace Instatus.Data
{
    public interface IMessageProcessor<TMessage>
    {
        void Start();
    }
}

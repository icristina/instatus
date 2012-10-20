using Autofac;
using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Integration.Autofac
{
    public class AutofacContainer : Instatus.Core.IContainer
    {
        private ILifetimeScope lifetimeScope;
        
        public T Resolve<T>()
        {
            return lifetimeScope.Resolve<T>();
        }

        public void Dispose()
        {
            lifetimeScope.Dispose();
        }

        public AutofacContainer(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }
    }
}

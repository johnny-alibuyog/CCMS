using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.UI.Infrastructure;
using Ninject;

namespace CCMS.UI.Infrastructure
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public DependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public object Resolve(Type type)
        {
            return _kernel.Get(type);
        }

        public T Resolve<T>()
        {
            return _kernel.Get<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _kernel.GetAll<T>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using CCMS.UI.Infrastructure;

namespace CCMS.UI.Bootstrappers.IoC
{
    public static class IoCBootstrapper
    {
        private static IDependencyResolver _resolver;

        public static IDependencyResolver Resolver
        {
            get
            {
                if (_resolver == null)
                    _resolver = Initialize();

                return _resolver;
            }
        }

        public static IDependencyResolver Initialize()
        {
            var kernel = new StandardKernel();
            kernel.Load(AppDomain.CurrentDomain.GetAssemblies());

            _resolver = new DependencyResolver(kernel);
            return _resolver;
        }
    }
}

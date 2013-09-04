using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using CCMS.UI.Infrastructure;

namespace CCMS.UI.Bootstrappers.IoC
{
    public static class IoC
    {
        public static IDependencyResolver Container
        {
            get { return IoCBootstrapper.Resolver; }
        }
    }
}

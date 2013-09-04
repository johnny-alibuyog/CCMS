using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.UI.Infrastructure
{
    public interface IDependencyResolver
    {
        object Resolve(Type type);
        T Resolve<T>();
        IEnumerable<T> ResolveAll<T>();
    }
}

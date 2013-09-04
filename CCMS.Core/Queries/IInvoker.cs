using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Queries
{
    public interface IInvoker
    {
        T Invoke<T>(IQuery<T> query);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Data.Queries
{
    public sealed class Invoker : SessionManager
    {
        public T Invoke<T>(Query<T> query)
        {
            query.Session = this.Session;
            query.Transaction = this.Transaction;
            return query.Execute();
        }
    }
}

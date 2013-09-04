using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Data.Queries
{
    public abstract class Query<T> : SessionManager
    {
        public abstract T Execute();
    }
}

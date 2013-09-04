using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Queries
{
    public interface IQuery<T>
    {
        T Execute();
        void Commit();
        void Rollback();
    }
}

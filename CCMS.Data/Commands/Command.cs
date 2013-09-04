using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Data.Common.Exceptions;
using CCMS.Data.Common.Exceptions;
using NHibernate;
using NHibernate.Validator.Engine;

namespace CCMS.Data.Commands
{
    public abstract class Command : SessionManager, IDisposable
    {
        public abstract void Execute();
    }

    public abstract class Command<T> : SessionManager, IDisposable
    {
        public abstract T Execute();
    }
}

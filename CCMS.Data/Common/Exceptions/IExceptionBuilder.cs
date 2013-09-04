using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Engine;

namespace CCMS.Data.Common.Exceptions
{
    public interface IExceptionBuilder<TException> where TException : Exception
    {
        TException Build(params InvalidValue[] invalidValues);
    }
}

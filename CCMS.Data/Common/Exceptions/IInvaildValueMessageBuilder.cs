using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Engine;

namespace CCMS.Data.Common.Exceptions
{
    public interface IInvalidValueMessageBuilder
    {
        string Build(params InvalidValue[] invalidValues);
        string Build(string key, params InvalidValue[] invalidValues);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Data.Configurations;
using NHibernate;
using NHibernate.Validator.Engine;

namespace CCMS.Data
{
    public interface ISessionProvider
    {
        AuditResolver AuditResolver { get; set; }
        ISession GetSharedSession();
        ISession ReleaseSharedSession();
        ISessionFactory SessionFactory { get; }
        ValidatorEngine ValidatorEngine { get; }
    }
}

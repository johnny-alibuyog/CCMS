using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class StaffValidation : ValidationDef<Staff>
    {
        public StaffValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Person)
                .NotNullable()
                .And.IsValid();

            Define(x => x.User)
                .NotNullable()
                .And.IsValid();
        }
    }
}

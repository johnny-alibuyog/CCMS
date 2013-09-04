using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class AuditValidation : ValidationDef<Audit>
    {
        public AuditValidation()
        {
            Define(x => x.CreatedBy)
                .MaxLength(25);

            Define(x => x.UpdatedBy)
                .MaxLength(25);

            Define(x => x.CreatedOn);

            Define(x => x.UpdatedOn);
        }
    }
}

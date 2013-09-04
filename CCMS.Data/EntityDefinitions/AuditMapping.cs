using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class AuditMapping : ComponentMap<Audit>
    {
        public AuditMapping()
        {
            Map(x => x.CreatedBy);

            Map(x => x.UpdatedBy);

            Map(x => x.CreatedOn);

            Map(x => x.UpdatedOn);
        }
    }
}

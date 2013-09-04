using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class BillingItemValidaiton : ValidationDef<BillingItem>
    {
        public BillingItemValidaiton()
        {
            Define(x => x.Id);

            //Define(x => x.Version);

            //Define(x => x.Audit);

            Define(x => x.Date);

            Define(x => x.Details)
                .MaxLength(250);

            Define(x => x.Billing);
                //.NotNullable();
        }
    }
}

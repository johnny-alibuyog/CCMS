using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class AdjustmentBillingItemValidation : ValidationDef<AdjustmentBillingItem>
    {
        public AdjustmentBillingItemValidation()
        {
            Define(x => x.Adjustment)
                .NotNullable()
                .And.IsValid();
        }
    }
}

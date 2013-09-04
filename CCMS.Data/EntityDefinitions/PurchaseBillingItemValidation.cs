using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class PurchaseBillingItemValidation : ValidationDef<PurchaseBillingItem>
    {
        public PurchaseBillingItemValidation()
        {
            Define(x => x.Purchase)
                .NotNullable()
                .And.IsValid();
        }
    }
}

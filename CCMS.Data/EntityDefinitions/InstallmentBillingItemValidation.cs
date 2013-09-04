using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class InstallmentBillingItemValidation : ValidationDef<InstallmentBillingItem>
    {
        public InstallmentBillingItemValidation()
        {
            Define(x => x.Installment)
                .NotNullable()
                .And.IsValid();
        }
    }
}

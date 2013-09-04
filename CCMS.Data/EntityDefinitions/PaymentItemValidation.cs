using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class PaymentItemValidation : ValidationDef<PaymentItem>
    {
        public PaymentItemValidation()
        {
            Define(x => x.Id);

            Define(x => x.Payment)
                .NotNullable();

            Define(x => x.Billing)
                .NotNullable();

            Define(x => x.Amount)
                .NotNullable();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class PaymentValidation : ValidationDef<Payment>
    {
        public PaymentValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.Billing)
                .NotNullable();

            Define(x => x.Date);

            Define(x => x.TransactionClassification);

            Define(x => x.Staff);

            Define(x => x.Amount)
                .NotNullable();

            //Define(x => x.PaymentItems)
            //    .HasValidElements();
        }
    }
}

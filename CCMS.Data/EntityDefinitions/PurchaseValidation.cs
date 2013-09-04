using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class PurchaseValidation : ValidationDef<Purchase>
    {
        public PurchaseValidation()
        {
            Define(x => x.Id);

            Define(x => x.CreditCard)
                .NotNullable();

            Define(x => x.Date);

            Define(x => x.TransactionClassification);

            Define(x => x.Staff);

            Define(x => x.Details)
                .MaxLength(250);

            Define(x => x.Amount)
                .NotNullable();
        }
    }
}

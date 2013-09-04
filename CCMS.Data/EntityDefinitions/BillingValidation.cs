using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class BillingValidation : ValidationDef<Billing>
    {
        public BillingValidation()
        {
            Define(x => x.Id);

            //Define(x => x.Version);

            //Define(x => x.Audit);

            Define(x => x.CreditCard)
                .NotNullable();

            Define(x => x.BillingStatus);

            Define(x => x.StartDate);

            Define(x => x.EndDate);

            Define(x => x.StatementDate);

            Define(x => x.DueDate);

            Define(x => x.PreviousBillingAmount);

            Define(x => x.PreviousPaymentAmount);

            Define(x => x.AdjustmentAmount);

            Define(x => x.CashAdvanceAmount);

            Define(x => x.ChargeAmount);

            Define(x => x.FeeAmount);

            Define(x => x.InstallmentAmount);

            Define(x => x.InterestAmount);

            Define(x => x.PurchaseAmount);

            Define(x => x.BillingAmount);

            Define(x => x.PaymentAmount);

            Define(x => x.SettlementBalance);

            Define(x => x.BillingItems);
        }
    }
}

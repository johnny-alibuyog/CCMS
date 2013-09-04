using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class CreditCardValidation : ValidationDef<CreditCard>
    {
        public CreditCardValidation()
        {
            Define(x => x.Id);

            Define(x => x.Version);

            Define(x => x.Audit);

            Define(x => x.AccountNumber)
                .NotNullableAndNotEmpty();

            Define(x => x.AccountName)
                .NotNullableAndNotEmpty();

            Define(x => x.CutOff);

            Define(x => x.IssueDate)
                .Satisfy(x => x != null)
                .WithMessage("Issue date is mandatory.");

            Define(x => x.ExpiryDate)
                .Satisfy(x => x != null)
                .WithMessage("Expiry date is mandatory.");

            Define(x => x.User)
                .NotNullable();

            Define(x => x.Provider)
                .NotNullable();

            Define(x => x.TransactionCurrency)
                .NotNullable();

            Define(x => x.CreditLimit)
                .NotNullable();

            Define(x => x.CashAdvanceLimit)
                .NotNullable();

            Define(x => x.OutstandingBalance);

            Define(x => x.AvailableCredit);

            Define(x => x.Billings);

            ValidateInstance.By((instance, context) =>
            {
                if (instance.CreditLimit != null && instance.OutstandingBalance != null && instance.CreditLimit < instance.OutstandingBalance)
                {
                    context.AddInvalid<CreditCard, Money>("Transaction exceeds credit limit. Increase credit limit to perform transaction.", x => x.CreditLimit);

                    return false;
                }

                return true;
            });
        }
    }
}

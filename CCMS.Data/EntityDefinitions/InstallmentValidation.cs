using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace CCMS.Data.EntityDefinitions
{
    public class InstallmentValidation : ValidationDef<Installment>
    {
        public InstallmentValidation()
        {
            Define(x => x.Id);

            Define(x => x.CreditCard)
                .NotNullable();

            Define(x => x.Date);

            Define(x => x.Details)
                .MaxLength(250);

            Define(x => x.TransactionClassification);

            Define(x => x.Staff);

            Define(x => x.Terms);

            Define(x => x.InterestRate);

            Define(x => x.Amortization)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Interest)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Amount)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Balance)
                .IsValid();
        }
    }
}

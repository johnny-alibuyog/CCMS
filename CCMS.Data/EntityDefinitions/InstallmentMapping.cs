using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class InstallmentMapping : ClassMap<Installment>
    {
        public InstallmentMapping()
        {
            Id(x => x.Id);

            References(x => x.CreditCard);

            Map(x => x.Date);

            Map(x => x.Details);

            References(x => x.TransactionClassification)
                .Fetch.Join();

            References(x => x.Staff)
                .Fetch.Join();

            Map(x => x.Terms);

            Map(x => x.InterestRate);

            Component<Money>(x => x.Amortization,
                MoneyMapping.Map("Amortization")
            );

            Component<Money>(x => x.Interest,
                MoneyMapping.Map("Interest")
            );

            Component<Money>(x => x.Amount,
                MoneyMapping.Map("Amount")
            );

            Component<Money>(x => x.Balance,
                MoneyMapping.Map("Balance")
            );
        }
    }
}

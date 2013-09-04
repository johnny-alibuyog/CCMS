using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class AdjustmentMapping : ClassMap<Adjustment>
    {
        public AdjustmentMapping()
        {
            Id(x => x.Id);

            References(x => x.CreditCard);

            Map(x => x.Date);

            Map(x => x.Details);

            References(x => x.TransactionClassification)
                .Fetch.Join();

            References(x => x.Staff)
                .Fetch.Join();

            Component<Money>(x => x.Amount,
                MoneyMapping.Map("Amount")
            );
        }
    }
}

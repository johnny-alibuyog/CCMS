using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class BillingItemMapping : ClassMap<BillingItem>
    {
        public BillingItemMapping()
        {
            //OptimisticLock.Version();

            Id(x => x.Id);

            //Version(x => x.Version);

            //Component(x => x.Audit);

            References(x => x.Billing);

            Map(x => x.Date);

            Map(x => x.Details);

            Component<Money>(x => x.Amount,
                MoneyMapping.Map("Amount")
            );

            DiscriminateSubClassesOnColumn("Discriminator")
                .Not.Nullable()
                .Length(5);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class PaymentItemMapping : ClassMap<PaymentItem>
    {
        public PaymentItemMapping()
        {
            Id(x => x.Id);

            References(x => x.Payment);

            References(x => x.Billing)
                .Fetch.Join();

            //Component(x => x.Amount)
            //    .ColumnPrefix("Amount");

            Component<Money>(x => x.Amount,
                MoneyMapping.Map("Amount")
            );

            //Id(x => x.Id, map =>
            //{
            //    map.Access(Accessor.ReadOnly);
            //    map.Generator(Generators.GuidComb);
            //});

            //ManyToOne(x => x.Payment);

            //Set(x => x.Purchases, map =>
            //{
            //    map.Inverse(true);
            //    map.Cascade(Cascade.All);
            //});

            //Property(x => x.Amount, map =>
            //{
            //    map.NotNullable(true);
            //});
        }
    }
}

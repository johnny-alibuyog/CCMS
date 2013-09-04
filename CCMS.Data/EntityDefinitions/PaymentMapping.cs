using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class PaymentMapping : ClassMap<Payment>
    {
        public PaymentMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            References(x => x.Billing);

            Map(x => x.Date);

            References(x => x.TransactionClassification)
                .Fetch.Join();

            References(x => x.Staff)
                .Fetch.Join();

            Component<Money>(x => x.Amount, 
                MoneyMapping.Map("Amount")
            );

            //HasMany(x => x.PaymentItems)
            //    .Access.CamelCaseField(Prefix.Underscore)
            //    .Cascade.AllDeleteOrphan()
            //    .Not.KeyNullable()
            //    .Not.KeyUpdate()
            //    .Inverse()
            //    .AsSet();

            //Id(x => x.Id, map =>
            //{
            //    map.Access(Accessor.ReadOnly);
            //    map.Generator(Generators.GuidComb);
            //});

            //Version(x => x.Version, map =>
            //{
            //    map.Access(Accessor.ReadOnly);
            //    map.Generated(VersionGeneration.Always);
            //});

            //ManyToOne(x => x.CreditCard);

            //Set(x => x.PaymentItems, map =>
            //{
            //    map.Inverse(true);
            //    map.Cascade(Cascade.All);
            //});

            //Property(x => x.PaymentDate, map =>
            //{
            //    map.NotNullable(true);
            //});

            //Property(x => x.Amount, map =>
            //{
            //    map.NotNullable(true);
            //});
        }
    }
}

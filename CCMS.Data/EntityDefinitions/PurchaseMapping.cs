using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class PurchaseMapping : ClassMap<Purchase>
    {
        public PurchaseMapping()
        {
            Id(x => x.Id);

            References(x => x.CreditCard);

            Map(x => x.Date);

            Map(x => x.Details);

            //Component(x => x.Amount)
            //    .ColumnPrefix("Amount");

            References(x => x.TransactionClassification)
                .Fetch.Join();

            References(x => x.Staff)
                .Fetch.Join();

            Component<Money>(x => x.Amount,
                MoneyMapping.Map("Amount")
            );

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

            //ManyToOne(x => x.PaymentItem);

            //ManyToOne(x => x.PaymentMode);

            //ManyToOne(x => x.PaymentStatus);

            //Set(x => x.PurchaseItems, map =>
            //{
            //    map.Inverse(true);
            //    map.Cascade(Cascade.All);
            //});
        }
    }
}

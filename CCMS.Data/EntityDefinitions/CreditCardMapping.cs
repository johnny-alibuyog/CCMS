using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class CreditCardMapping : ClassMap<CreditCard>
    {
        public CreditCardMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            Map(x => x.AccountNumber);

            Map(x => x.AccountName);

            Map(x => x.CutOff);

            Map(x => x.IssueDate);

            Map(x => x.ExpiryDate);

            References(x => x.User);

            References(x => x.Provider);

            References(x => x.TransactionCurrency);

            Component<Money>(x => x.CreditLimit, 
                MoneyMapping.Map("CreditLimit")
            );
            
            Component<Money>(x => x.CashAdvanceLimit,
                MoneyMapping.Map("CashAdvanceLimit")
            );

            Component<Money>(x => x.OutstandingBalance,
                MoneyMapping.Map("OutstandingBalance")
            );

            Component<Money>(x => x.AvailableCredit,
                MoneyMapping.Map("AvailableCredit")
            );

            HasMany(x => x.Billings)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();

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

            //Property(x => x.AccountNumber, map =>
            //{
            //    map.Length(17);
            //    map.Unique(true);
            //    map.NotNullable(true);
            //});

            //Property(x => x.AccountName, map =>
            //{
            //    map.Length(150);
            //    map.NotNullable(true);
            //});

            //ManyToOne(x => x.Company);

            //Property(x => x.CreditLimit, map =>
            //{
            //    map.NotNullable(true);
            //});

            //ManyToOne(x => x.User);

            //Set(x => x.Purchases, map =>
            //{
            //    map.Inverse(true);
            //    map.Cascade(Cascade.All);
            //});

            //Set(x => x.Payments, map =>
            //{
            //    map.Inverse(true);
            //    map.Cascade(Cascade.All);
            //});
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class BillingMapping : ClassMap<Billing>
    {
        public BillingMapping()
        {
            //OptimisticLock.Version();

            Id(x => x.Id);

            //Version(x => x.Version);

            //Component(x => x.Audit);

            References(x => x.CreditCard);

            Map(x => x.BillingStatus);

            Map(x => x.StartDate);

            Map(x => x.EndDate);

            Map(x => x.StatementDate);

            Map(x => x.DueDate);

            Component<Money>(x => x.PreviousBillingAmount,
                MoneyMapping.Map("PreviousBillingAmount")
            );

            Component<Money>(x => x.PreviousPaymentAmount,
                MoneyMapping.Map("PreviousPaymentAmount")
            );

            Component<Money>(x => x.AdjustmentAmount,
                MoneyMapping.Map("AdjustmentAmount")
            );

            Component<Money>(x => x.CashAdvanceAmount,
                MoneyMapping.Map("CashAdvanceAmount")
            );

            Component<Money>(x => x.ChargeAmount,
                MoneyMapping.Map("ChargeAmount")
            );

            Component<Money>(x => x.FeeAmount,
                MoneyMapping.Map("FeeAmount")
            );

            Component<Money>(x => x.InstallmentAmount,
                MoneyMapping.Map("InstallmentAmount")
            );

            Component<Money>(x => x.InterestAmount,
                MoneyMapping.Map("InterestAmount")
            );

            Component<Money>(x => x.PurchaseAmount,
                MoneyMapping.Map("PurchaseAmount")
            );

            Component<Money>(x => x.BillingAmount,
                MoneyMapping.Map("BillingAmount")
            );

            Component<Money>(x => x.PaymentAmount,
                MoneyMapping.Map("PaymentAmout")
            );

            Component<Money>(x => x.SettlementBalance,
                MoneyMapping.Map("SettlementBalance")
            );
            //.Access.CamelCaseField(Prefix.Underscore);

            HasMany(x => x.BillingItems)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();

            HasMany(x => x.Payments)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();

            HasMany(x => x.PreviousBillings)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();
        }
    }
}

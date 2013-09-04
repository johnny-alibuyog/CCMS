using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class CreditCardProviderMapping : ClassMap<CreditCardProvider>
    {
        public CreditCardProviderMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            Map(x => x.Name);

            Map(x => x.InterestRate);

            HasMany(x => x.ComputationSettings)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .KeyColumn("ProviderId")
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();


            //References(x => x.FinanceChargeSetting)
            //    .Cascade.All();

            //References(x => x.InterestSetting)
            //    .Cascade.All();

            //References(x => x.LateChargeSetting)
            //    .Cascade.All();

            //References(x => x.OverlimitFeeSetting)
            //    .Cascade.All();

            //References(x => x.ServiceFeeSetting)
            //    .Cascade.All();

            //Id(x => x.Id, map =>
            //{
            //    map.Length(5);
            //    map.Access(Accessor.ReadOnly);
            //    map.Generator(Generators.Assigned);
            //});

            //Version(x => x.Version, map =>
            //{
            //    map.Access(Accessor.ReadOnly);
            //    map.Generated(VersionGeneration.Always);
            //});

            //Property(x => x.Name, map =>
            //{
            //    map.Length(150);
            //    map.Unique(true);
            //    map.NotNullable(true);
            //});
        }
    }
}

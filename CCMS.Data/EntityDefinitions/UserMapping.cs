using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            OptimisticLock.Version();

            Id(x => x.Id);

            Version(x => x.Version);

            Component(x => x.Audit);

            Map(x => x.Username)
                .Unique();

            Map(x => x.Password);

            Component(x => x.Person);

            HasMany(x => x.CreditCards)
                .Cascade.AllDeleteOrphan()
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

            //Property(x => x.Password, map =>
            //{
            //    map.Length(20);
            //    map.NotNullable(true);
            //});

            //Component(x => x.Person);

            //Set(x => x.CreditCards, map =>
            //{
            //    map.Inverse(true);
            //    map.Cascade(Cascade.All);
            //});
        }
    }
}

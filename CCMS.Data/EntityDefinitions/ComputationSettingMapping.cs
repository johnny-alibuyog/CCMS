using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class ComputationSettingMapping : ClassMap<ComputationSetting>
    {
        public ComputationSettingMapping()
        {
            Id(x => x.Id);

            References(x => x.Provider);

            Component<Money>(x => x.MinimumAmount,
                MoneyMapping.Map("MinimumAmount")
            );

            Map(x => x.Rate);

            DiscriminateSubClassesOnColumn("Discriminator")
                .Not.Nullable()
                .Length(50);
        }
    }
}

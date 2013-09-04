using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class LateChargeSettingMapping : SubclassMap<LateChargeSetting>
    {
        public LateChargeSettingMapping()
        {
            DiscriminatorValue("LateChargeSetting");

            //HasOne(x => x.Provider)
            //    .PropertyRef(x => x.LateChargeSetting);
        }
    }
}

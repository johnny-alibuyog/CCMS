using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class FinanceChargeSettingMapping : SubclassMap<FinanceChargeSetting>
    {
        public FinanceChargeSettingMapping()
        {
            DiscriminatorValue("FinanceChargeSetting");

            //HasOne(x => x.Provider)
            //    .PropertyRef(x => x.FinanceChargeSetting);
        }
    }
}

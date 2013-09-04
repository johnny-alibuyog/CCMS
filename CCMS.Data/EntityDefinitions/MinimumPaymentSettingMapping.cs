using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class MinimumPaymentSettingMapping: SubclassMap<MinimumPaymentSetting>
    {
        public MinimumPaymentSettingMapping()
        {
            DiscriminatorValue("MinimumPaymentSetting");

            //HasOne(x => x.Provider)
            //    .PropertyRef(x => x.LateChargeSetting);
        }
    }
}

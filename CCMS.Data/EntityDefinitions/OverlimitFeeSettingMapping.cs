using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class OverlimitFeeSettingMapping : SubclassMap<OverlimitFeeSetting>
    {
        public OverlimitFeeSettingMapping()
        {
            DiscriminatorValue("OverlimitFeeSetting");

            //HasOne(x => x.Provider)
            //    .PropertyRef(x => x.OverlimitFeeSetting);
        }
    }
}

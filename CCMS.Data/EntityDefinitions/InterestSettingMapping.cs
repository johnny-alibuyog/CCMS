using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class InterestSettingMapping : SubclassMap<InterestSetting>
    {
        public InterestSettingMapping()
        {
            DiscriminatorValue("InterestSetting");

            //HasOne(x => x.Provider)
            //    .PropertyRef(x => x.InterestSetting);
        }
    }
}

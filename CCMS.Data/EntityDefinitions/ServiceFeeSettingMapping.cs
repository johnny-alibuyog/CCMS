using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class ServiceFeeSettingMapping : SubclassMap<ServiceFeeSetting>
    {
        public ServiceFeeSettingMapping()
        {
            DiscriminatorValue("ServiceFeeSetting");

            //HasOne(x => x.Provider)
            //    .PropertyRef(x => x.ServiceFeeSetting);
        }
    }
}

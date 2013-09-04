using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class ChargeBillingItemMapping : SubclassMap<ChargeBillingItem>
    {
        public ChargeBillingItemMapping()
        {
            DiscriminatorValue("CBI");
        }
    }
}

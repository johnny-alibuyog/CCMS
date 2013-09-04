using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class AdjustmentBillingItemMapping : SubclassMap<AdjustmentBillingItem>
    {
        public AdjustmentBillingItemMapping()
        {
            DiscriminatorValue("ABI");

            References(x => x.Adjustment)
                .Cascade.All()
                .Fetch.Join();
        }
    }
}

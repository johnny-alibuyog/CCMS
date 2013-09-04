using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class InterestBillingItemMapping : SubclassMap<InterestBillingItem>
    {
        public InterestBillingItemMapping()
        {
            DiscriminatorValue("IrBI");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class PurchaseBillingItemMapping : SubclassMap<PurchaseBillingItem>
    {
        public PurchaseBillingItemMapping()
        {
            DiscriminatorValue("PBI");

            References(x => x.Purchase)
                .Cascade.All()
                .Fetch.Join();
        }
    }
}

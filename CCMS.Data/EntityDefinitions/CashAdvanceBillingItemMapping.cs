using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class CashAdvanceBillingItemMapping : SubclassMap<CashAdvanceBillingItem>
    {
        public CashAdvanceBillingItemMapping()
        {
            DiscriminatorValue("CABI");

            References(x => x.CashAdvance)
                .Cascade.All()
                .Fetch.Join();
        }
    }
}

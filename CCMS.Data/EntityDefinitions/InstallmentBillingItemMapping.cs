using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class InstallmentBillingItemMapping : SubclassMap<InstallmentBillingItem>
    {
        public InstallmentBillingItemMapping()
        {
            DiscriminatorValue("IBI");

            References(x => x.Installment)
                .Cascade.All()
                .Fetch.Join();
        }
    }
}

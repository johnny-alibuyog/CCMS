using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class TransactionClassificationMapping : ClassMap<TransactionClassification>
    {
        public TransactionClassificationMapping()
        {
            Id(x => x.Id);

            Map(x => x.Name)
                .Unique();
        }
    }
}
